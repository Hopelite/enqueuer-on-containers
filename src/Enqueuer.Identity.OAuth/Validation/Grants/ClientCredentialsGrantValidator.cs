using Enqueuer.Identity.OAuth.Models.Grants;

namespace Enqueuer.Identity.OAuth.Validation.Grants;

public class ClientCredentialsGrantValidator : GrantValidatorBase<ClientCredentialsGrant>
{
    protected override Task AuthorizeGrantAsync(ClientCredentialsGrant grant, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
