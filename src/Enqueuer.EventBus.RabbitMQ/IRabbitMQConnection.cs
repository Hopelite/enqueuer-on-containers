using RabbitMQ.Client;

namespace Enqueuer.EventBus.RabbitMQ;

/// <summary>
/// Defines the RabbitMQ connection.
/// </summary>
public interface IRabbitMQConnection : IDisposable
{
    /// <summary>
    /// Whether bus connection is connected and opened.
    /// </summary>
    bool IsConnected { get; }

    /// <summary>
    /// Gets AMQP model for this RabbitMQ connection.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown, if <see cref="IsConnected"/> is false.</exception>
    IModel CreateModel();

    /// <summary>
    /// Connects this instance to RabbitMQ message broker.
    /// </summary>
    void Connect();
}
