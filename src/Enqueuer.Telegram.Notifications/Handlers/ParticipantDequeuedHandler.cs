using Enqueuer.EventBus.Abstractions;
using Enqueuer.Identity.Contract.V1;
using Enqueuer.Queueing.Contract.V1.Events;
using Enqueuer.Telegram.Notifications.Localization;
using Enqueuer.Telegram.Notifications.Services;
using Enqueuer.Telegram.Shared.Localization;
using System.Globalization;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace Enqueuer.Telegram.Notifications.Handlers;

public class ParticipantDequeuedHandler(
    IChatConfigurationService chatConfigurationService,
    ILocalizationProvider localizationProvider,
    IIdentityClient identityClient,
    ITelegramBotClient telegramClient)
    : IntegrationEventHandlerBase<ParticipantDequeuedEvent>
{
    private readonly IChatConfigurationService _chatConfigurationService = chatConfigurationService;
    private readonly ILocalizationProvider _localizationProvider = localizationProvider;
    private readonly IIdentityClient _identityClient = identityClient;
    private readonly ITelegramBotClient _telegramClient = telegramClient;

    public override async Task HandleAsync(ParticipantDequeuedEvent @event, CancellationToken cancellationToken)
    {
        var userInfo = await _identityClient.GetUserInfoAsync(@event.ParticipantId, cancellationToken);

        var chatConfiguration = await _chatConfigurationService.GetChatConfigurationAsync(@event.GroupId, cancellationToken: cancellationToken);
        var chatCulture = new CultureInfo(chatConfiguration.MessageLanguageCode);

        // TODO: handle the user full name
        var message = await _localizationProvider.GetMessageAsync(
            key: NotificationKeys.UserDequeuedFromQueueNotification,
            messageParameters: new MessageParameters(chatCulture, userInfo.FirstName, @event.QueueName),
            cancellationToken);

        await _telegramClient.SendTextMessageAsync(
            @event.GroupId,
            message,
            parseMode: ParseMode.Html,
            cancellationToken: cancellationToken);
    }
}
