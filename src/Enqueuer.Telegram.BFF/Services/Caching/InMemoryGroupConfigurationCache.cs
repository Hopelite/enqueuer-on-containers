using Enqueuer.Telegram.BFF.Core.Configuration;
using Enqueuer.Telegram.BFF.Core.Models.Configuration;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Enqueuer.Telegram.BFF.Services.Caching;

public class InMemoryGroupConfigurationCache : IChatConfigurationCache
{
    private const int ConfigurationEntrySize = (8 + 8 + 16) + 32;

    // TODO: replace this cache since it shares memory
    private readonly IMemoryCache _memoryCache;
    private readonly MemoryCacheEntryOptions _cacheOptions;

    public InMemoryGroupConfigurationCache(IMemoryCache memoryCache, IOptions<ConfigurationCacheOptions> options)
    {
        _memoryCache = memoryCache;
        var cacheOptions = options.Value;
        _cacheOptions = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(cacheOptions.SlidingExpiration)
            .SetAbsoluteExpiration(cacheOptions.AbsoluteExpiration)
            .SetSize(ConfigurationEntrySize);
    }

    public ValueTask<ChatMessagingConfiguration?> GetGroupConfigurationAsync(long groupId, CancellationToken cancellationToken)
    {
        var configuration = _memoryCache.Get<ChatMessagingConfiguration>(groupId);
        return ValueTask.FromResult(configuration);
    }

    public ValueTask SetGroupConfigurationAsync(ChatMessagingConfiguration configuration, CancellationToken cancellationToken)
    {
        _memoryCache.Set(configuration.ChatId, configuration, _cacheOptions);
        return ValueTask.CompletedTask;
    }
}
