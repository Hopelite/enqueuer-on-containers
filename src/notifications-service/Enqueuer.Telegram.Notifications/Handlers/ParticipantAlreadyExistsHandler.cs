using System.Globalization;
using Enqueuer.EventBus.Abstractions;
using Enqueuer.Identity.Contract.V1;
using Enqueuer.Queueing.Contract.V1.Events.RejectedEvents;
using Enqueuer.Telegram.Notifications.Extensions;
using Enqueuer.Telegram.Notifications.Localization;
using Enqueuer.Telegram.Notifications.Services;
using Enqueuer.Telegram.Shared.Localization;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace Enqueuer.Telegram.Notifications.Handlers;

public class ParticipantAlreadyExistsHandler(
    IChatConfigurationService chatConfigurationService,
    ILocalizationProvider localizationProvider,
    IIdentityClient identityClient,
    ITelegramBotClient telegramClient)
    : IntegrationEventHandlerBase<ParticipantAlreadyExistsEvent>
{
    private readonly IChatConfigurationService _chatConfigurationService = chatConfigurationService;
    private readonly ILocalizationProvider _localizationProvider = localizationProvider;
    private readonly IIdentityClient _identityClient = identityClient;
    private readonly ITelegramBotClient _telegramClient = telegramClient;

    public override async Task HandleAsync(ParticipantAlreadyExistsEvent @event, CancellationToken cancellationToken)
    {
        var userInfo = await _identityClient.GetUserInfoAsync(@event.ParticipantId, cancellationToken);

        var chatConfiguration = await _chatConfigurationService.GetChatConfigurationAsync(@event.GroupId, cancellationToken: cancellationToken);
        var chatCulture = new CultureInfo(chatConfiguration.MessageLanguageCode);

        var message = _localizationProvider.GetMessage(
            NotificationKeys.UserAlreadyParticipatesInQueueNotification,
            new MessageParameters(chatCulture, userInfo.GetFullName(), @event.QueueName));

        await _telegramClient.SendTextMessageAsync(
            @event.GroupId,
            message,
            parseMode: ParseMode.Html,
            cancellationToken: cancellationToken);
    }
}
