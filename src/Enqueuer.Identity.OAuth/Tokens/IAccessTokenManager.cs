using Enqueuer.Identity.OAuth.Exceptions;
using Enqueuer.Identity.OAuth.Models.Grants;

namespace Enqueuer.Identity.OAuth.Tokens;

public interface IAccessTokenManager
{
    /// <summary>
    /// Requests an <see cref="AccessToken"/> using the provided <paramref name="authorizationGrant"/>.
    /// </summary>
    /// <exception cref="AuthorizationException">Thrown in case of any authorization error.</exception>
    Task<AccessToken> RequestAccessTokenAsync(IAuthorizationGrant authorizationGrant, CancellationToken cancellationToken);
}
