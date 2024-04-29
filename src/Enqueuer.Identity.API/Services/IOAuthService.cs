using Enqueuer.Identity.API.Parameters;
using Enqueuer.Identity.API.Services.Grants;

namespace Enqueuer.Identity.API.Services;

public interface IOAuthService
{
    AccessToken GetAccessTokenAsync(IAuthorizationGrant grant, ScopeCollection scopes);
}
