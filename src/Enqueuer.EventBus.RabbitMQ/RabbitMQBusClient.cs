using Enqueuer.EventBus.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace Enqueuer.EventBus.RabbitMQ;

public class RabbitMQBusClient : BackgroundService, IEventBusClient
{
    private const string ExchangeName = "enqueuer_event_bus";
    private readonly IConnectionFactory _connectionFactory;
    private readonly RabbitMQConfiguration _configuration;
    private readonly IServiceProvider _serviceProvider;
    private readonly SubscriptionList _subscriptionList;
    private readonly ILogger<RabbitMQBusClient> _logger;
    private IModel? _eventListeningChannel;
    private IConnection? _busConnection;


    private const int MaxRetries = 5;

    public RabbitMQBusClient(
        IConnectionFactory connectionFactory,
        IOptions<SubscriptionList> subscriptionList,
        RabbitMQConfiguration configuration,
        IServiceProvider serviceProvider,
        ILogger<RabbitMQBusClient> logger)
    {
        _connectionFactory = connectionFactory;
        _configuration = configuration;
        _serviceProvider = serviceProvider;
        _subscriptionList = subscriptionList.Value;
        _logger = logger;
    }

    public Task PublishAsync(IIntegrationEvent @event, CancellationToken cancellationToken)
    {
        if (_busConnection == null || !_busConnection.IsOpen)
        {
            throw new InvalidOperationException("Bus connection hasn't been opened yet.");
        }

        // TODO: consider to avoid GetType() calls
        var routingKey = @event.GetType().Name;

        var retryPolicy = Policy.Handle<BrokerUnreachableException>()
            .Or<SocketException>()
            .WaitAndRetry(MaxRetries, attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)), (ex, timeElapsed) =>
            {
                _logger.LogWarning("Could not publish '{EventName}' event after {TimeElapsed}s: {Error}", routingKey, timeElapsed.TotalSeconds, ex.Message);
            });

        // TODO: Consider caching channels and binding queues in a single place
        using var channel = _busConnection.CreateModel();
        channel.ExchangeDeclare(ExchangeName, type: "direct");

        channel.QueueDeclare(queue: routingKey, durable: true, exclusive: false, autoDelete: false);
        channel.QueueBind(queue: routingKey, exchange: ExchangeName, routingKey: routingKey);

        var body = JsonSerializer.SerializeToUtf8Bytes(@event, @event.GetType());

        retryPolicy.Execute(() =>
        {
            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            channel.BasicPublish(
                ExchangeName,
                routingKey: routingKey,
                mandatory: true,
                properties,
                body);
        });

        return Task.CompletedTask;
    }

    public override void Dispose()
    {
        GC.SuppressFinalize(this);
        base.Dispose();
        _busConnection?.Dispose();
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return Task.Factory.StartNew(() =>
        {
            try
            {
                _logger.LogInformation("Starting RabbitMQ connection.");

                _busConnection = _connectionFactory.CreateConnection();

                _logger.LogInformation("Creating event listening channel.");

                _eventListeningChannel = _busConnection.CreateModel();
                _eventListeningChannel.CallbackException += LogBusException;

                _eventListeningChannel.ExchangeDeclare(ExchangeName, type: "direct");

                _logger.LogInformation("Subscribing to queues.");

                _eventListeningChannel.QueueDeclare(
                    queue: _configuration.SubscriptionClientName,
                    durable: true,
                    exclusive: false,
                    autoDelete: false);

                var consumer = new AsyncEventingBasicConsumer(_eventListeningChannel);
                consumer.Received += HandleIncomingMessage;

                _eventListeningChannel.BasicConsume(
                    queue: _configuration.SubscriptionClientName,
                    autoAck: false,
                    consumer);

                foreach (var (eventName, _) in _subscriptionList)
                {
                    _eventListeningChannel.QueueBind(queue: _configuration.SubscriptionClientName, exchange: ExchangeName, routingKey: eventName);
                }

                _logger.LogInformation("Successfully started consuming events.");
            }
            catch (Exception ex)
            {
                // Non-working bus connection, while critical, should not stop the other application functionality
                _logger.LogCritical(ex, "An error occured during the RabbitMQ connection start.");
            }
        }, stoppingToken);
    }

    private async Task HandleIncomingMessage(object sender, BasicDeliverEventArgs eventArgs)
    {
        var eventName = eventArgs.RoutingKey;

        var message = Encoding.UTF8.GetString(eventArgs.Body.Span);
        if (!_subscriptionList.TryGet(eventName, out var eventType))
        {
            return;
        }

        await using var scope = _serviceProvider.CreateAsyncScope();

        var integrationEvent = JsonSerializer.Deserialize(message, eventType) as IIntegrationEvent;
        foreach (var handler in scope.ServiceProvider.GetKeyedServices<IIntegrationEventHandler>(eventType))
        {
            // TODO: pass via Channel
            await handler.HandleAsync(integrationEvent, CancellationToken.None);
        }

        _eventListeningChannel!.BasicAck(eventArgs.DeliveryTag, multiple: false);
    }

    private void LogBusException(object? sender, CallbackExceptionEventArgs eventArgs)
    {
        _logger.LogWarning(eventArgs.Exception, "An error occured in bus connection.");
    }
}
