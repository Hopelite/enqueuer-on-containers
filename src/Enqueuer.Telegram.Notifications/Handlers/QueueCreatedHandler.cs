using Enqueuer.Telegram.Notifications.Localization;
using Enqueuer.Telegram.Notifications.Notifications;
using Enqueuer.Telegram.Shared.Markup;
using Enqueuer.Telegram.Shared.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Enqueuer.Telegram.Notifications.Handlers;

internal class QueueCreatedHandler : INotificationHandler<QueueCreatedNotification>
{
    private readonly ILocalizationProvider _localizationProvider;
    private readonly IInlineMarkupBuilder _markupBuilder;
    private readonly ITelegramBotClient _telegramClient;

    public QueueCreatedHandler(
        ILocalizationProvider localizationProvider,
        IInlineMarkupBuilder markupBuilder,
        ITelegramBotClient telegramClient)
    {
        _localizationProvider = localizationProvider;
        _markupBuilder = markupBuilder;
        _telegramClient = telegramClient;
    }

    public Task HandleAsync(QueueCreatedNotification notification, CancellationToken cancellationToken)
    {
        // TODO: retrieve chat localization by ID

        var message = _localizationProvider.GetMessage(
            NotificationKeys.QueueCreated,
            new NotificationParameters(new System.Globalization.CultureInfo("en-US"), notification.CreatorId.ToString(), notification.QueueName));

        var markup = _markupBuilder.Add(serializer =>
        {
            var callbackData = new CallbackData
            {
                Command = "eqm", // TODO: possibly replace with enum
                QueueId = notification.QueueId,
            };

            var jsonData = serializer.Serialize(callbackData);

            // TODO: replace with localized "Enqueue me" message
            return InlineKeyboardButton.WithCallbackData("Enqueue me!", jsonData);
        }).Build();

        // TODO: consider working with forums
        // TODO: retrive chat configuration to determine whether to add notification or not
        // TODO: add telegram client decorator with auto-configured parameters (like parse mode)
        return _telegramClient.SendTextMessageAsync(
            notification.ChatId,
            message,
            parseMode: ParseMode.Html,
            replyMarkup: markup,
            cancellationToken: cancellationToken);
    }
}
