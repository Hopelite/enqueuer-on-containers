using Enqueuer.Identity.OAuth.Models.Grants;

namespace Enqueuer.Identity.OAuth.Models;

public class AccessTokenRequest
{
    public AccessTokenRequest(IAuthorizationGrant authorizationGrant)
    {
        AuthorizationGrant = authorizationGrant;
    }

    /// <summary>
    /// The authorization grant to authorize the client with.
    /// </summary>
    public IAuthorizationGrant AuthorizationGrant { get; }
}
