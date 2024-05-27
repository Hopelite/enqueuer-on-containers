using Enqueuer.Identity.OAuth.Models.Grants;
using Enqueuer.Identity.OAuth.Storage;

namespace Enqueuer.Identity.OAuth.Validation.Grants;

public class AuthorizationCodeGrantValidator : GrantValidatorBase<AuthorizationCodeGrant>
{
    private readonly IClientCredentialsStorage _credentialsStorage;

    public AuthorizationCodeGrantValidator(IClientCredentialsStorage credentialsStorage)
    {
        _credentialsStorage = credentialsStorage;
    }

    protected override async Task AuthorizeGrantAsync(AuthorizationCodeGrant grant, CancellationToken cancellationToken)
    {
        if (grant.ClientId != null)
        {
            await _credentialsStorage.GetClientSecretAsync(grant.ClientId, cancellationToken);
        }


    }
}
