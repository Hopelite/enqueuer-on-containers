using Enqueuer.Identity.OAuth.Exceptions;
using Enqueuer.Identity.OAuth.Models.Grants;
using Enqueuer.Identity.OAuth.Tokens;

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
