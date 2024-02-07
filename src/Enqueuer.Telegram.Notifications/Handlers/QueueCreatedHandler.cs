using Enqueuer.EventBus.Abstractions;
using Enqueuer.Queueing.Contract.V1.Events;
using Enqueuer.Telegram.Notifications.Localization;
using Enqueuer.Telegram.Notifications.Services;
using Enqueuer.Telegram.Shared.Markup;
using Enqueuer.Telegram.Shared.Types;
using System.Globalization;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Enqueuer.Telegram.Notifications.Handlers;

public class QueueCreatedHandler(
    IChatConfigurationService chatConfigurationService,
    ILocalizationProvider localizationProvider,
    IInlineMarkupBuilder markupBuilder,
    ITelegramBotClient telegramClient)
    : IntegrationEventHandlerBase<QueueCreatedEvent>
{
    private readonly IChatConfigurationService _chatConfigurationService = chatConfigurationService;
    private readonly ILocalizationProvider _localizationProvider = localizationProvider;
    private readonly IInlineMarkupBuilder _markupBuilder = markupBuilder;
    private readonly ITelegramBotClient _telegramClient = telegramClient;

    public override async Task HandleAsync(QueueCreatedEvent @event, CancellationToken cancellationToken)
    {
        var chatConfiguration = await _chatConfigurationService.GetChatNotificationsAsync(@event.LocationId, cancellationToken);
        var chatCulture = new CultureInfo(chatConfiguration.NotificationsLanguageCode);

        var message = await _localizationProvider.GetMessageAsync(
            key: NotificationKeys.QueueCreatedNotification,
            messageParameters: new MessageParameters(chatCulture, "Vadzim", @event.QueueName),
            cancellationToken);

        var buttonText = await _localizationProvider.GetMessageAsync(NotificationKeys.EnqueueMeButton, new MessageParameters(chatCulture), cancellationToken);
        var markup = _markupBuilder.Add(serializer =>
        {
            var callbackData = new CallbackData
            {
                Command = "eqm", // TODO: possibly replace with enum
                QueueId = @event.QueueId,
            };

            var jsonData = serializer.Serialize(callbackData);
            return InlineKeyboardButton.WithCallbackData(buttonText, jsonData);
        }).Build();

        // TODO: consider working with forums
        // TODO: retrive chat configuration to determine whether to send notification with sound or not
        // TODO: add telegram client decorator with auto-configured parameters (like parse mode)
        await _telegramClient.SendTextMessageAsync(
            @event.LocationId,
            message,
            parseMode: ParseMode.Html,
            replyMarkup: markup,
            cancellationToken: cancellationToken);
    }
}
