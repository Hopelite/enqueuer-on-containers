using Enqueuer.Telegram.Notifications.Notifications;

namespace Enqueuer.Telegram.Notifications.Handlers;

internal interface INotificationHandler<T> where T: ITelegramNotification
{
    Task HandleAsync(T notification, CancellationToken cancellationToken);
}
