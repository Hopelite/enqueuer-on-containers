using Enqueuer.Identity.OAuth.Models.Grants;
using Enqueuer.Identity.OAuth.Tokens;

namespace Enqueuer.Identity.OAuth.Authorization;

public abstract class GrantAuthorizerBase<TGrant> : IGrantAuthorizer
    where TGrant : IAuthorizationGrant
{
    protected abstract Task<AccessTokenContext> AuthorizeGrantAsync(TGrant grant, CancellationToken cancellationToken);

    public Task<AccessTokenContext> AuthorizeAsync(IAuthorizationGrant grant, CancellationToken cancellationToken)
    {
        return AuthorizeGrantAsync((TGrant)grant, cancellationToken);
    }
}
