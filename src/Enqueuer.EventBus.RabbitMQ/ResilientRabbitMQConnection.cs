using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Enqueuer.EventBus.RabbitMQ;

/// <summary>
/// Keeps bus connection open and reconnects on failure.
/// </summary>
public class ResilientRabbitMQConnection : IRabbitMQConnection
{
    private readonly IConnectionFactory _connectionFactory;
    private readonly ILogger<ResilientRabbitMQConnection> _logger;
    private readonly object _balanceObject = new();
    private IConnection? _connection;
    private bool _isDisposed = false;

    public ResilientRabbitMQConnection(IConnectionFactory connectionFactory, ILogger<ResilientRabbitMQConnection> logger)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
    }

    public bool IsConnected
        => _connection != null
        && _connection.IsOpen
        && !_isDisposed;

    public void Connect()
    {
        if (IsConnected)
        {
            return;
        }

        lock (_balanceObject)
        {
            _connection = _connectionFactory
                .CreateConnection();
        }

        _connection.CallbackException += ReconnectOnException;
        _connection.ConnectionShutdown += ReconnectOnShutdown;
    }

    public IModel CreateModel()
    {
        if (!IsConnected)
        {
            throw new InvalidOperationException("Cannot create AMQP model from non-started connection.");
        }

        return _connection!.CreateModel();
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_isDisposed)
        {
            if (disposing && _connection != null)
            {
                _connection.CallbackException -= ReconnectOnException;
                _connection.ConnectionShutdown -= ReconnectOnShutdown;
                _connection.Dispose();
            }

            _isDisposed = true;
        }
    }

    private void ReconnectOnException(object? sender, CallbackExceptionEventArgs eventArgs)
    {
        if (_isDisposed)
        {
            return;
        }

        _logger.LogWarning(eventArgs.Exception, "An error occured in bus connection. Trying to reconnect.");
        Connect();
    }

    private void ReconnectOnShutdown(object? sender, ShutdownEventArgs eventArgs)
    {
        if (_isDisposed)
        {
            return;
        }

        _logger.LogWarning(eventArgs.Exception, "An error caused connection shutdown. Trying to reconnect.");
        Connect();
    }
}
