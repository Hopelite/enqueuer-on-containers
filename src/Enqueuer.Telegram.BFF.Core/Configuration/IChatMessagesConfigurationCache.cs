using Enqueuer.Telegram.BFF.Core.Models.Configuration;

namespace Enqueuer.Telegram.BFF.Core.Configuration;

public interface IChatMessagesConfigurationCache
{
    ValueTask<ChatMessagingConfiguration?> GetGroupConfigurationAsync(long groupId, CancellationToken cancellationToken);

    ValueTask SetGroupConfigurationAsync(ChatMessagingConfiguration configuration, CancellationToken cancellationToken);
}
