using Enqueuer.Telegram.Notifications.Persistence.Entities;

namespace Enqueuer.Telegram.Notifications.Services;

public interface IChatConfigurationService
{
    Task ConfigureChatNotificationsAsync(ChatNotificationsConfiguration chatConfiguration, CancellationToken cancellationToken);

    Task<ChatNotificationsConfiguration> GetChatNotificationsAsync(long chatId, CancellationToken cancellationToken);
}
