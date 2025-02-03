using Enqueuer.Identity.Contract.V1.OAuth.Configuration;
using Enqueuer.Identity.Contract.V1.OAuth.RequestHandlers;
using Enqueuer.Queueing.Contract.V1;
using Enqueuer.Queueing.Contract.V1.Configuration;
using Enqueuer.Telegram.BFF.Messages.Handlers;
using Enqueuer.Telegram.BFF.Services.MessageHandling;
using Enqueuer.Telegram.Notifications.Contract.V1;
using Enqueuer.Telegram.Notifications.Contract.V1.Configuration;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers all non-abstract implementations of the <see cref="IMessageHandler"/> in all referenced assemblies.
    /// </summary>
    public static IServiceCollection AddMessageHandlers(this IServiceCollection services)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        foreach (var assembly in assemblies)
        {
            var types = assembly.GetTypes()
                .Where(type => type.IsClass && !type.IsAbstract && typeof(IMessageHandler).IsAssignableFrom(type));

            foreach (var type in types)
            {
                if (type == typeof(MessageHandlerErrorHandling))
                {
                    continue;
                }

                services.AddScoped(type);
            }
        }

        return services;
    }

    public static IHttpClientBuilder AddQueueingClient(this WebApplicationBuilder builder, string name = "Enqueuer Queueing Client")
    {
        builder.Services.AddTransient<IOptions<ClientCredentialsAuthorizationOptions<IQueueingClient>>>(services => services.GetRequiredService<IOptions<QueueingClientOptions>>());
        builder.Services.Configure<QueueingClientOptions>(builder.Configuration.GetRequiredSection("QueueingClient"), configure =>
        {
            configure.BindNonPublicProperties = true;
        });

        builder.Services.AddTransient<ClientCredentialsTokenHandler<IQueueingClient>>();
        return builder.Services.AddHttpClient<IQueueingClient, QueueingClient>(name, (serviceProvider, client) =>
        {
            var options = serviceProvider.GetRequiredService<IOptions<QueueingClientOptions>>().Value;
            client.BaseAddress = options.BaseAddress;
        })
        .AddHttpMessageHandler(serviceProvider =>
        {
            return serviceProvider.GetRequiredService<ClientCredentialsTokenHandler<IQueueingClient>>();
        });
    }

    public static IHttpClientBuilder AddChatConfigurationClient(this WebApplicationBuilder builder, string name = "Enqueuer Chat Configuration Client")
    {
        builder.Services.Configure<ChatConfigurationClientOptions>(builder.Configuration.GetRequiredSection("ChatConfigurationClient"));
        return builder.Services.AddHttpClient<IChatConfigurationClient, ChatConfigurationClient>(name, (serviceProvider, client) =>
        {
            var options = serviceProvider.GetRequiredService<IOptions<ChatConfigurationClientOptions>>().Value;
            client.BaseAddress = options.BaseAddress;
        });
    }
}
