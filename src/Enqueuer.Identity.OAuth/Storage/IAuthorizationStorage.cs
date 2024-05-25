using Enqueuer.Identity.OAuth.Models;

namespace Enqueuer.Identity.OAuth.Storage;

public interface IAuthorizationStorage
{
    /// <summary>
    /// Generates the authorization code and stores it in cache.
    /// </summary>
    Task<AuthorizationCode> RequestCodeAsync(CancellationToken cancellationToken);
}
