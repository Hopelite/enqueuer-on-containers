using Microsoft.Extensions.Configuration;

namespace Enqueuer.EventBus.RabbitMQ;

public class RabbitMQConfiguration
{
    public RabbitMQConfiguration(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("EventBus");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new ArgumentException("Event bus connection string is required.");
        }

        ConnectionString = new Uri(connectionString);
    }

    public Uri ConnectionString { get; }
}
