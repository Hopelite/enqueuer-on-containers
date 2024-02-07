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
        SubscriptionClientName = configuration.GetRequiredSection("EventBus")["SubscriptionClientName"]
            ?? throw new ArgumentException("SubscriptionClientName is a required configuration value.");
    }

    public Uri ConnectionString { get; }

    public string SubscriptionClientName { get; }
}
