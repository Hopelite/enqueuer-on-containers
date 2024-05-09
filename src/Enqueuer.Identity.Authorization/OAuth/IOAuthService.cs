using Enqueuer.Identity.Authorization.Grants;
using Enqueuer.Identity.Authorization.Models;

namespace Enqueuer.Identity.Authorization.OAuth;

public interface IOAuthService
{
    /// <summary>
    /// Authorizes the <paramref name="grant"/> and gets the access token with the specified <paramref name="scopes"/>.
    /// </summary>
    /// <remarks>If some <paramref name="scopes"/> are invalid, they got omitted and the accepted scopes are specified in the token.</remarks>
    Task<AccessToken> GetAccessTokenAsync(IAuthorizationGrant grant, IReadOnlyCollection<Scope> scopes, CancellationToken cancellationToken);
}
