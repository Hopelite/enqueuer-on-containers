using Enqueuer.Identity.Contract.V1.Models;
using Enqueuer.Telegram.BFF.Core.Models.Common;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Enqueuer.Telegram.BFF.Services.Caching;

public class InMemoryUserInfoCache : IUserInfoCache
{
    private readonly IMemoryCache _memoryCache;
    private readonly MemoryCacheEntryOptions _cacheOptions;

    public InMemoryUserInfoCache(IMemoryCache memoryCache, IOptions<ConfigurationCacheOptions> options)
    {
        _memoryCache = memoryCache;
        var cacheOptions = options.Value;
        _cacheOptions = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(cacheOptions.SlidingExpiration)
            .SetAbsoluteExpiration(cacheOptions.AbsoluteExpiration);
            //.SetSize(ConfigurationEntrySize);
    }

    public ValueTask SetUserInfoAsync(User userInfo, CancellationToken cancellationToken)
    {
        _memoryCache.Set(userInfo.Id, userInfo, _cacheOptions);
        return ValueTask.CompletedTask;
    }

    public ValueTask<User?> GetUserInfoAsync(long userId, CancellationToken cancellationToken)
    {
        var userInfo = _memoryCache.Get<User>(userId);
        return ValueTask.FromResult(userInfo);
    }
}
