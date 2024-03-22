using Enqueuer.Telegram.Shared.Configuration;
using Enqueuer.Telegram.Shared.Exceptions;
using Enqueuer.Telegram.Shared.Markup;
using Enqueuer.Telegram.Shared.Serialization;
using Microsoft.Extensions.Options;
using Telegram.Bot;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
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
}
