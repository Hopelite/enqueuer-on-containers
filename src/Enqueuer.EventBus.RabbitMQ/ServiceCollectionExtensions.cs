using Enqueuer.EventBus.Abstractions;
using Enqueuer.EventBus.RabbitMQ;
using RabbitMQ.Client;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IEventBusBuilder AddRabbitMQClient(this IServiceCollection services)
    {
        services.AddSingleton<RabbitMQConfiguration>();
        services.AddTransient<IConnectionFactory, ConnectionFactory>(services =>
        {
            var busConfiguration = services.GetRequiredService<RabbitMQConfiguration>();
            return new ConnectionFactory()
            {
                Uri = busConfiguration.ConnectionString,
                DispatchConsumersAsync = true,
            };
        });

        services.AddSingleton<IEventBusClient, RabbitMQBusClient>();
        services.AddHostedService(services => (RabbitMQBusClient)services.GetRequiredService<IEventBusClient>());

        return new EventBusBuilder(services);
    }

}
