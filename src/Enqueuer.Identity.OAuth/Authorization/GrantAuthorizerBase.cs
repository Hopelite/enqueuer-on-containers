using Enqueuer.Identity.OAuth.Tokens;
using Enqueuer.OAuth.Core.Tokens.Grants;

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
