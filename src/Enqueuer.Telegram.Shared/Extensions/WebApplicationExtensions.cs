using Enqueuer.Telegram.Shared.Configuration;
using Enqueuer.Telegram.Shared.Exceptions;
using Enqueuer.Telegram.Shared.Markup;
using Enqueuer.Telegram.Shared.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Telegram.Bot;

namespace Enqueuer.Telegram.Shared.Extensions;

public static class WebApplicationExtensions
{
    /// <summary>
    /// Registers <see cref="ITelegramBotClient"/> along with <see cref="TelegramBotClientOptions"/>, <see cref="IInlineMarkupBuilder"/> and <see cref="IDataSerializer"/> implementations.
    /// </summary>
    public static WebApplicationBuilder AddTelegramClient(this WebApplicationBuilder builder, string configurationSection = "TelegramClient")
    {
        builder.Services.Configure<TelegramBotClientConfiguration>(builder.Configuration.GetRequiredSection(configurationSection));

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
