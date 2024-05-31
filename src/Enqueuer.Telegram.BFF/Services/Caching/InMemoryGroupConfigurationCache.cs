using Enqueuer.Telegram.BFF.Core.Configuration;
using Enqueuer.Telegram.BFF.Core.Models.Configuration;
using System.Collections.Concurrent;

namespace Enqueuer.Telegram.BFF.Services.Caching;

public class InMemoryGroupConfigurationCache : IChatMessagesConfigurationCache
{
    private readonly ConcurrentDictionary<long, ChatMessagingConfiguration> _configs = new();

    public ValueTask<ChatMessagingConfiguration?> GetGroupConfigurationAsync(long groupId, CancellationToken cancellationToken)
    {
        _configs.TryGetValue(groupId, out var configuration);
        return ValueTask.FromResult(configuration);
    }

    public ValueTask SetGroupConfigurationAsync(ChatMessagingConfiguration configuration, CancellationToken cancellationToken)
    {
        _configs.AddOrUpdate(configuration.ChatId, configuration, (_, existing) =>
        {
            return configuration;
        });

        return ValueTask.CompletedTask;
    }
}
