namespace Enqueuer.EventBus.RabbitMQ;

public class RabbitMQConfiguration
{
    public required string HostName { get; init; }

    public string? Username { get; init; }

    public string? Password { get; init; }
}
