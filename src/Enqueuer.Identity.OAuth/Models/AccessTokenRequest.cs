using Enqueuer.OAuth.Core.Tokens.Grants;

namespace Enqueuer.Identity.OAuth.Models;

public class AccessTokenRequest
{
    public AccessTokenRequest(IAuthorizationGrant authorizationGrant)
    {
        AuthorizationGrant = authorizationGrant ?? throw new ArgumentNullException(nameof(authorizationGrant));
    }

    /// <summary>
    /// The authorization grant to authorize the client with.
    /// </summary>
    public IAuthorizationGrant AuthorizationGrant { get; }
}
