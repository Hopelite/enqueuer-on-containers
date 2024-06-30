using System.Globalization;
using Enqueuer.EventBus.Abstractions;
using Enqueuer.Queueing.Contract.V1.Events.RejectedEvents;
using Enqueuer.Telegram.Notifications.Localization;
using Enqueuer.Telegram.Notifications.Services;
using Enqueuer.Telegram.Shared.Localization;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace Enqueuer.Telegram.Notifications.Handlers;

public class QueueDoesNotExistHandler(
    IChatConfigurationService chatConfigurationService,
    ILocalizationProvider localizationProvider,
    ITelegramBotClient telegramClient)
    : IntegrationEventHandlerBase<QueueDoesNotExistEvent>
{
    private readonly IChatConfigurationService _chatConfigurationService = chatConfigurationService;
    private readonly ILocalizationProvider _localizationProvider = localizationProvider;
    private readonly ITelegramBotClient _telegramClient = telegramClient;

    public override async Task HandleAsync(QueueDoesNotExistEvent @event, CancellationToken cancellationToken)
    {
        var chatConfiguration = await _chatConfigurationService.GetChatConfigurationAsync(@event.GroupId, cancellationToken: cancellationToken);
        var chatCulture = new CultureInfo(chatConfiguration.MessageLanguageCode);

        var message = _localizationProvider.GetMessage(
            NotificationKeys.QueueDoesNotExistNotification,
            new MessageParameters(chatCulture, @event.QueueName));

        await _telegramClient.SendTextMessageAsync(
            @event.GroupId,
            message,
            parseMode: ParseMode.Html,
            cancellationToken: cancellationToken);
    }
}
