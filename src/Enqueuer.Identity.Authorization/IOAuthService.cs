using Enqueuer.Identity.Authorization.Grants;
using Enqueuer.Identity.Authorization.Models;

namespace Enqueuer.Identity.Authorization;

public interface IOAuthService
{
    AccessToken GetAccessTokenAsync(IAuthorizationGrant grant, IReadOnlyCollection<Scope> scopes);
}
