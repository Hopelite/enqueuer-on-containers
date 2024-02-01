using Enqueuer.EventBus.Abstractions;
using Enqueuer.EventBus.RabbitMQ;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Enqueuer.Queueing.API.Extensions;

public static class ServicesConfiguration
{
    public static WebApplicationBuilder AddRabbitMQ(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<RabbitMQConfiguration>();
        builder.Services.AddSingleton<IRabbitMQConnection>(services =>
        {
            var logger = services.GetRequiredService<ILogger<ResilientRabbitMQConnection>>();
            var busConfiguration = services.GetRequiredService<RabbitMQConfiguration>();

            var factory = new ConnectionFactory()
            {
                Uri = busConfiguration.ConnectionString,
                DispatchConsumersAsync = true,
            };

            return new ResilientRabbitMQConnection(factory, logger);
        });

        builder.Services.AddSingleton<IEventBusClient, RabbitMQBusClient>();
        builder.Services.AddHostedService(services => (RabbitMQBusClient)services.GetRequiredService<IEventBusClient>());

        return builder;
    }
}
