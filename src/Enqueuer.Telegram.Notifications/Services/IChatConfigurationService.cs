using Enqueuer.Telegram.Notifications.Contract.V1.Models;

namespace Enqueuer.Telegram.Notifications.Services;

public interface IChatConfigurationService
{
    Task ConfigureChatNotificationsAsync(ChatNotificationsConfiguration chatConfiguration, CancellationToken cancellationToken);

    Task<ChatNotificationsConfiguration> GetChatConfigurationAsync(long chatId, CancellationToken cancellationToken);
}
