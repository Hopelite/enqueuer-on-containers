using Enqueuer.Identity.OAuth.Tokens;
using Enqueuer.OAuth.Core.Exceptions;
using Enqueuer.OAuth.Core.Tokens.Grants;

namespace Enqueuer.Identity.OAuth.Authorization;

public interface IGrantAuthorizer
{
    /// <summary>
    /// Validates and authorizes the <paramref name="grant"/>.
    /// </summary>
    /// <returns><see cref="AccessTokenContext"/> containing neccessary data for access token generation.</returns>
    /// <exception cref="AuthorizationException">Thrown in case the grant cannot be authorized with or is invalid.</exception>
    Task<AccessTokenContext> AuthorizeAsync(IAuthorizationGrant grant, CancellationToken cancellationToken);
}
