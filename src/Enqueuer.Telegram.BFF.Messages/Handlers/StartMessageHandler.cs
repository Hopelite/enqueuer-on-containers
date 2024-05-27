using Enqueuer.Telegram.BFF.Core.Models.Messages;
using Enqueuer.Telegram.Shared.Localization;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Enqueuer.Telegram.BFF.Messages.Handlers;

public class StartMessageHandler(
    ITelegramBotClient telegramClient,
    ILocalizationProvider localizationProvider,
    ILogger<StartMessageHandler> logger) : MessageHandlerBase(telegramClient, localizationProvider)
{
    private readonly ILogger<StartMessageHandler> _logger = logger;

    public override async Task HandleAsync(MessageContext messageContext, CancellationToken cancellationToken)
    {
        // TODO: get and cache chat localization from Notifications service

    }
}
