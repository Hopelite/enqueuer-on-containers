using Enqueuer.EventBus.Abstractions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System.Net.Sockets;
using System.Text.Json;

namespace Enqueuer.EventBus.RabbitMQ;

public class RabbitMQBusClient : IEventBusClient, IHostedService
{
    private const string ExchangeName = "enqueuer_event_bus";
    private const int MaxRetries = 5;

    private readonly IRabbitMQConnection _busConnection;
    private readonly ILogger<RabbitMQBusClient> _logger;

    public RabbitMQBusClient(IRabbitMQConnection rabbitMQConnection, ILogger<RabbitMQBusClient> logger)
    {
        _busConnection = rabbitMQConnection;
        _logger = logger;
    }

    public Task PublishAsync(IIntegrationEvent @event, CancellationToken cancellationToken)
    {
        if (!_busConnection.IsConnected)
        {
            _busConnection.Connect();
        }

        var retryPolicy = Policy.Handle<BrokerUnreachableException>()
            .Or<SocketException>()
            .WaitAndRetry(MaxRetries, attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)), (ex, timeElapsed) =>
            {
                _logger.LogWarning("Could not publish '{EventName}' event after {TimeElapsed}s: {Error}", @event.Name, timeElapsed.TotalSeconds, ex.Message);
            });

        // TODO: Consider caching channels and binding queues in a single place
        using var channel = _busConnection.CreateModel();
        channel.ExchangeDeclare(ExchangeName, type: "direct", durable: true);
        channel.QueueDeclare(queue: @event.Name, durable: true, exclusive: false, autoDelete: false);
        channel.QueueBind(queue: @event.Name, exchange: ExchangeName, routingKey: @event.Name);

        var body = JsonSerializer.SerializeToUtf8Bytes(@event, @event.GetType());

        retryPolicy.Execute(() =>
        {
            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            channel.BasicPublish(
                ExchangeName,
                routingKey: @event.Name,
                mandatory: true,
                properties,
                body);
        });

        return Task.CompletedTask;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting RabbitMQ connection.");

        _busConnection.Connect();

        _logger.LogInformation("Successfully connected to RabbitMQ message broker.");

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
