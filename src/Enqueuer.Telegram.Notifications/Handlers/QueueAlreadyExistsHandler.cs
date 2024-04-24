using Enqueuer.EventBus.Abstractions;
using Enqueuer.Queueing.Contract.V1.Events.RejectedEvents;
using Enqueuer.Telegram.Notifications.Localization;
using Enqueuer.Telegram.Notifications.Services;
using Enqueuer.Telegram.Shared.Localization;
using System.Globalization;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace Enqueuer.Telegram.Notifications.Handlers;

public class QueueAlreadyExistsHandler(
    IChatConfigurationService chatConfigurationService,
    ILocalizationProvider localizationProvider,
    ITelegramBotClient telegramClient)
    : IntegrationEventHandlerBase<QueueAlreadyExistsEvent>
{
    private readonly IChatConfigurationService _chatConfigurationService = chatConfigurationService;
    private readonly ILocalizationProvider _localizationProvider = localizationProvider;
    private readonly ITelegramBotClient _telegramClient = telegramClient;

    public override async Task HandleAsync(QueueAlreadyExistsEvent @event, CancellationToken cancellationToken)
    {
        // TODO: possibly move to base class
        var chatConfiguration = await _chatConfigurationService.GetChatConfigurationAsync(@event.GroupId, cancellationToken);
        var chatCulture = new CultureInfo(chatConfiguration.NotificationsLanguageCode);

        var message = await _localizationProvider.GetMessageAsync(
            key: NotificationKeys.QueueAlreadyExistsNotification,
            messageParameters: new MessageParameters(chatCulture, @event.QueueName),
            cancellationToken);

        await _telegramClient.SendTextMessageAsync(
            @event.GroupId,
            message,
            parseMode: ParseMode.Html,
            cancellationToken: cancellationToken);
    }
}
