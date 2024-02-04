using Enqueuer.EventBus.Abstractions;
using Enqueuer.Queueing.Contract.V1.Events;
using Enqueuer.Telegram.Notifications.Localization;
using Enqueuer.Telegram.Shared.Markup;
using Enqueuer.Telegram.Shared.Types;
using System.Globalization;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Enqueuer.Telegram.Notifications.Handlers;

public class QueueCreatedHandler(ILocalizationProvider localizationProvider, IInlineMarkupBuilder markupBuilder, ITelegramBotClient telegramClient)
    : IntegrationEventHandlerBase<QueueCreatedEvent>
{
    private readonly ILocalizationProvider _localizationProvider = localizationProvider;
    private readonly IInlineMarkupBuilder _markupBuilder = markupBuilder;
    private readonly ITelegramBotClient _telegramClient = telegramClient;

    public override Task HandleAsync(QueueCreatedEvent @event, CancellationToken cancellationToken)
    {
        // TODO: retrieve chat localization by ID
        var chatCulture = new CultureInfo("en-US");

        var message = _localizationProvider.GetMessage(
            NotificationKeys.QueueCreatedNotification,
            new NotificationParameters(chatCulture, "Vadzim", @event.QueueName));

        var buttonText = _localizationProvider.GetMessage(NotificationKeys.EnqueueMeButton, new NotificationParameters(chatCulture));
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
        return _telegramClient.SendTextMessageAsync(
            @event.LocationId,
            message,
            parseMode: ParseMode.Html,
            replyMarkup: markup,
            cancellationToken: cancellationToken);
    }
}
