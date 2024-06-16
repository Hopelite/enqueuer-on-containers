using Enqueuer.OAuth.Core.Exceptions;
using Enqueuer.OAuth.Core.Tokens;
using Enqueuer.OAuth.Core.Tokens.Grants;

namespace Enqueuer.Identity.OAuth.Tokens;

public interface IAccessTokenManager
{
    /// <summary>
    /// Requests an <see cref="AccessToken"/> using the provided <paramref name="authorizationGrant"/>.
    /// </summary>
    /// <exception cref="AuthorizationException">Thrown in case of any authorization error.</exception>
    Task<AccessToken> RequestAccessTokenAsync(IAuthorizationGrant authorizationGrant, CancellationToken cancellationToken);
}
