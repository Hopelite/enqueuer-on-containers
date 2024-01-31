using Enqueuer.EventBus.Abstractions;
using Enqueuer.EventBus.RabbitMQ;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Enqueuer.Queueing.API.Extensions;

public static class ServicesConfiguration
{
    public static WebApplicationBuilder AddRabbitMQ(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<RabbitMQConfiguration>(builder.Configuration.GetRequiredSection("RabbitMQ"));
        builder.Services.AddSingleton<IRabbitMQConnection>(services =>
        {
            var logger = services.GetRequiredService<ILogger<ResilientRabbitMQConnection>>();
            var busConfiguration = services.GetRequiredService<IOptions<RabbitMQConfiguration>>().Value;

            var factory = new ConnectionFactory()
            {
                HostName = busConfiguration.HostName,
                UserName = GetValueOrDefault(busConfiguration.Username, ConnectionFactory.DefaultUser),
                Password = GetValueOrDefault(busConfiguration.Password, ConnectionFactory.DefaultPass),
                DispatchConsumersAsync = true,
            };

            return new ResilientRabbitMQConnection(factory, logger);
        });

        builder.Services.AddSingleton<IEventBusClient, RabbitMQBusClient>();
        builder.Services.AddHostedService(services => (RabbitMQBusClient)services.GetRequiredService<IEventBusClient>());

        return builder;
    }

    private static string GetValueOrDefault(string? value, string defaultValue)
    {
        return string.IsNullOrWhiteSpace(value) ? defaultValue : value;
    }
}
