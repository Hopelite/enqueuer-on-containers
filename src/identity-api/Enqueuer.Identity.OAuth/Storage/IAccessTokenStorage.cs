using Enqueuer.OAuth.Core.Tokens;

namespace Enqueuer.Identity.OAuth.Storage;

public interface IAccessTokenStorage
{
    /// <summary>
    /// Temporarily stores the <paramref name="accessToken"/>.
    /// </summary>
    ValueTask SaveAccessTokenAsync(AccessToken accessToken, CancellationToken cancellationToken);
}
