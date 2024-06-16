using Enqueuer.Identity.OAuth.Storage;
using Enqueuer.OAuth.Core.Tokens;
using Microsoft.Extensions.Caching.Memory;

namespace Enqueuer.Identity.API.Services;

public class InMemoryTokenStorage : IAccessTokenStorage
{
    private readonly IMemoryCache _memoryCache;

    public InMemoryTokenStorage(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    public ValueTask SaveAccessTokenAsync(AccessToken accessToken, CancellationToken cancellationToken)
    {
        _memoryCache.Set(accessToken.Value, accessToken, accessToken.ExpiresIn);
        return ValueTask.CompletedTask;
    }
}
