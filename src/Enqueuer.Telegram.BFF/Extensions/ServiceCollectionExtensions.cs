using Enqueuer.Queueing.Contract.V1;
using Enqueuer.Queueing.Contract.V1.Configuration;
using Enqueuer.Telegram.BFF.Extensions.RequestHandlers;
using Enqueuer.Telegram.BFF.Messages.Handlers;
using Enqueuer.Telegram.Shared.Configuration;
using Enqueuer.Telegram.Shared.Exceptions;
using Enqueuer.Telegram.Shared.Markup;
using Enqueuer.Telegram.Shared.Serialization;
using Microsoft.Extensions.Options;
using Telegram.Bot;

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
                services.AddScoped(type);
            }
        }

        return services;
    }

    public static WebApplicationBuilder AddTelegramClient(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<TelegramBotClientConfiguration>(builder.Configuration.GetRequiredSection("TelegramClient"));

        builder.Services.AddHttpClient(nameof(TelegramBotClient))
            .AddTypedClient<ITelegramBotClient>((httpClient, serviceProvider) =>
            {
                var configuration = serviceProvider.GetRequiredService<IOptions<TelegramBotClientConfiguration>>().Value;
                return new TelegramBotClient(configuration.AccessToken, httpClient)
                {
                    ExceptionsParser = new TelegramExceptionsParser(),
                };
            });

        builder.Services.AddTransient<IInlineMarkupBuilder, InlineMarkupBuilder>()
            .AddTransient<IDataSerializer, JsonDataSerializer>();

        return builder;
    }

    public static IHttpClientBuilder AddQueueingClient(this WebApplicationBuilder builder, string name = "Enqueuer Queueing Client")
    {
        builder.Services.AddTransient<AccessTokenHandler>();
        builder.Services.Configure<QueueingClientOptions>(builder.Configuration.GetRequiredSection("QueueingClient"));
        return builder.Services.AddHttpClient<IQueueingClient, QueueingClient>(name, (serviceProvider, client) =>
        {
            var options = serviceProvider.GetRequiredService<IOptions<QueueingClientOptions>>().Value;
            client.BaseAddress = options.BaseAddress;
        })
        .AddHttpMessageHandler(serviceProvider =>
        {
            return serviceProvider.GetRequiredService<AccessTokenHandler>();
        });
    }
}
