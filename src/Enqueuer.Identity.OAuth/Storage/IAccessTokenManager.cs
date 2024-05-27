using Enqueuer.Identity.OAuth.Exceptions;
using Enqueuer.Identity.OAuth.Models;
using Enqueuer.Identity.OAuth.Models.Grants;

namespace Enqueuer.Identity.OAuth.Storage;

public interface IAccessTokenManager
{
    /// <summary>
    /// Generates new access token for the <paramref name="authorizationGrant"/>.
    /// </summary>
    /// <exception cref="AuthorizationException">Thrown in case of any authorization error.</exception>
    Task<AccessToken> GetAccessTokenAsync(IAuthorizationGrant authorizationGrant, CancellationToken cancellationToken);
}
