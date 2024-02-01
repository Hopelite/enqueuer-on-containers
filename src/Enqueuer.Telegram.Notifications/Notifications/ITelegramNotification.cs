namespace Enqueuer.Telegram.Notifications.Notifications;

internal interface ITelegramNotification
{
    long ChatId { get; }
}
