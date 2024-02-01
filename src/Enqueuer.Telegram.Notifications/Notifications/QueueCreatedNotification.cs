namespace Enqueuer.Telegram.Notifications.Notifications;

internal class QueueCreatedNotification : ITelegramNotification
{
    public long ChatId { get; }

    public long CreatorId { get; }

    public int QueueId { get; }

    public string QueueName { get; } = null!;
}
