using Enqueuer.EventBus.Abstractions;
using Enqueuer.Identity.Contract.V1.Events;
using Enqueuer.Telegram.Notifications.Services;
using Enqueuer.Telegram.Shared.Localization;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;

namespace Enqueuer.Telegram.Notifications.Handlers;

public class UserAuthorizationRequestedHandler(
    IChatConfigurationService chatConfigurationService,
    ILocalizationProvider localizationProvider,
    ITelegramBotClient telegramClient) : IntegrationEventHandlerBase<UserAuthorizationRequestedEvent>
{
    private readonly IChatConfigurationService _chatConfigurationService = chatConfigurationService;
    private readonly ILocalizationProvider _localizationProvider = localizationProvider;
    private readonly ITelegramBotClient _telegramClient = telegramClient;

    public override async Task HandleAsync(UserAuthorizationRequestedEvent @event, CancellationToken cancellationToken)
    {
        var url = new Uri("https://f63d-89-64-81-51.ngrok-free.app/authorization/callback");

        var button = InlineKeyboardButton.WithLoginUrl("Authorize", new LoginUrl() { Url = url.AbsoluteUri, RequestWriteAccess = true });

        await _telegramClient.SendTextMessageAsync(
                chatId: @event.UserId,
                text: "Please, authorize this bot below:",
                parseMode: ParseMode.Html,
                replyMarkup: new InlineKeyboardMarkup(button),
                cancellationToken: cancellationToken);
    }
}
