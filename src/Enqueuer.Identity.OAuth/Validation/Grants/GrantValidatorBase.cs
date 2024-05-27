using Enqueuer.Identity.OAuth.Models.Grants;

namespace Enqueuer.Identity.OAuth.Validation.Grants;

public abstract class GrantValidatorBase<TGrant> : IGrantValidator
    where TGrant : IAuthorizationGrant
{
    protected abstract Task AuthorizeGrantAsync(TGrant grant, CancellationToken cancellationToken);

    public Task AuthorizeAsync(IAuthorizationGrant grant, CancellationToken cancellationToken)
    {
        return AuthorizeGrantAsync((TGrant)grant, cancellationToken);
    }
}
