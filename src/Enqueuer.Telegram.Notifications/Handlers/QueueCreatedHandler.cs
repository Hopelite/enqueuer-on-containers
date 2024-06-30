using System.Globalization;
using Enqueuer.EventBus.Abstractions;
using Enqueuer.Identity.Contract.V1;
using Enqueuer.Queueing.Contract.V1.Events;
using Enqueuer.Telegram.Notifications.Extensions;
using Enqueuer.Telegram.Notifications.Localization;
using Enqueuer.Telegram.Notifications.Services;
using Enqueuer.Telegram.Shared.Localization;
using Enqueuer.Telegram.Shared.Markup;
using Enqueuer.Telegram.Shared.Types;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Enqueuer.Telegram.Notifications.Handlers;

public class QueueCreatedHandler(
    IChatConfigurationService chatConfigurationService,
    ILocalizationProvider localizationProvider,
    IInlineMarkupBuilder markupBuilder,
    IIdentityClient identityClient,
    ITelegramBotClient telegramClient)
    : IntegrationEventHandlerBase<QueueCreatedEvent>
{
    private readonly IChatConfigurationService _chatConfigurationService = chatConfigurationService;
    private readonly ILocalizationProvider _localizationProvider = localizationProvider;
    private readonly IInlineMarkupBuilder _markupBuilder = markupBuilder;
    private readonly IIdentityClient _identityClient = identityClient;
    private readonly ITelegramBotClient _telegramClient = telegramClient;

    public override async Task HandleAsync(QueueCreatedEvent @event, CancellationToken cancellationToken)
    {
        var userInfo = await _identityClient.GetUserInfoAsync(@event.CreatorId, cancellationToken);

        var chatConfiguration = await _chatConfigurationService.GetChatConfigurationAsync(@event.GroupId, cancellationToken: cancellationToken);
        var chatCulture = new CultureInfo(chatConfiguration.MessageLanguageCode);

        var message = _localizationProvider.GetMessage(
            NotificationKeys.QueueCreatedNotification,
            new MessageParameters(chatCulture, userInfo.GetFullName(), @event.QueueName));

        var buttonText = _localizationProvider.GetMessage(NotificationKeys.EnqueueMeButton, new MessageParameters(chatCulture));
        var markup = _markupBuilder.Add(serializer =>
        {
            var callbackData = new CallbackData
            {
                Command = "eqm", // TODO: possibly replace with enum
                //QueueId = @event.QueueId, // TODO: provide internal queue id 
            };

            var jsonData = serializer.Serialize(callbackData);
            return InlineKeyboardButton.WithCallbackData(buttonText, jsonData);
        }).Build();

        // TODO: consider working with forums
        // TODO: retrive chat configuration to determine whether to send notification with sound or not
        // TODO: add telegram client decorator with auto-configured parameters (like parse mode)
        await _telegramClient.SendTextMessageAsync(
            @event.GroupId,
            message,
            parseMode: ParseMode.Html,
            replyMarkup: markup,
            cancellationToken: cancellationToken);
    }
}
