using Enqueuer.Identity.API.Parameters;
using Enqueuer.Identity.API.Services.Grants;

namespace Enqueuer.Identity.API.Services;

public interface IAuthorizationService
{
    AccessToken GetAccessToken(IAuthorizationGrant grant, ScopeCollection scopes);
}
