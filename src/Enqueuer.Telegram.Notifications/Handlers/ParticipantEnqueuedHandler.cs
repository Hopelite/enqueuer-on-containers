using Enqueuer.EventBus.Abstractions;
using Enqueuer.Queueing.Contract.V1.Events;
using Enqueuer.Telegram.Notifications.Localization;
using Enqueuer.Telegram.Notifications.Services;
using Enqueuer.Telegram.Shared.Localization;
using System.Globalization;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace Enqueuer.Telegram.Notifications.Handlers;

public class ParticipantEnqueuedHandler(
    IChatConfigurationService chatConfigurationService,
    ILocalizationProvider localizationProvider,
    ITelegramBotClient telegramClient)
    : IntegrationEventHandlerBase<ParticipantEnqueuedEvent>
{
    private readonly IChatConfigurationService _chatConfigurationService = chatConfigurationService;
    private readonly ILocalizationProvider _localizationProvider = localizationProvider;
    private readonly ITelegramBotClient _telegramClient = telegramClient;

    public override async Task HandleAsync(ParticipantEnqueuedEvent @event, CancellationToken cancellationToken)
    {
        var chatConfiguration = await _chatConfigurationService.GetChatConfigurationAsync(@event.GroupId, cancellationToken: cancellationToken);
        var chatCulture = new CultureInfo(chatConfiguration.MessageLanguageCode);

        var message = await _localizationProvider.GetMessageAsync(
            key: NotificationKeys.UserEnqueuedAtPositionNotification,
            messageParameters: new MessageParameters(chatCulture, "Vadzim", @event.QueueName, @event.Position.ToString()),
            cancellationToken);

        await _telegramClient.SendTextMessageAsync(
            @event.GroupId,
            message,
            parseMode: ParseMode.Html,
            cancellationToken: cancellationToken);
    }
}
