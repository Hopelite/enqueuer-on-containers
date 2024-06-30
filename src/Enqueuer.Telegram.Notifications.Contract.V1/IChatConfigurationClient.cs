using System.Threading;
using System.Threading.Tasks;
using Enqueuer.Telegram.Notifications.Contract.V1.Models;

namespace Enqueuer.Telegram.Notifications.Contract.V1
{
    public interface IChatConfigurationClient
    {
        /// <summary>
        /// Gets existing <see cref="ChatNotificationsConfiguration"/> for chat with the specified <paramref name="chatId"/>
        /// or creates a new one, setting the group language code to <paramref name="languageCode"/> if not null and is supported.
        /// </summary>
        Task<ChatNotificationsConfiguration> GetNewOrExistingConfigurationAsync(long chatId, string? languageCode, CancellationToken cancellationToken);

        Task ConfigureChatNotificationsAsync(ChatNotificationsConfiguration chatNotificationsConfiguration, CancellationToken cancellationToken);
    }
}
