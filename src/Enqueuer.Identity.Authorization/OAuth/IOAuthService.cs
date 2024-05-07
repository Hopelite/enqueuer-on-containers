using Enqueuer.Identity.Authorization.Grants;
using Enqueuer.Identity.Authorization.Models;

namespace Enqueuer.Identity.Authorization.OAuth;

public interface IOAuthService
{
    Task<AccessToken> GetAccessTokenAsync(IAuthorizationGrant grant, IReadOnlyCollection<Scope> scopes, CancellationToken cancellationToken);
}
