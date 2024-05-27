using Enqueuer.Identity.OAuth.Models;
using Enqueuer.Identity.OAuth.Models.Grants;
using Enqueuer.Identity.OAuth.Validation.Grants;

namespace Enqueuer.Identity.OAuth.Storage;

public class AccessTokenManager : IAccessTokenManager
{
    private readonly IGrantValidatorFactory _grantValidatorFactory;

    public AccessTokenManager(IGrantValidatorFactory grantValidatorFactory)
    {
        _grantValidatorFactory = grantValidatorFactory;
    }

    public async Task<AccessToken> GetAccessTokenAsync(IAuthorizationGrant authorizationGrant, CancellationToken cancellationToken)
    {
        var grantValidator = _grantValidatorFactory.GetGrantValidator(authorizationGrant.Type);
        await grantValidator.AuthorizeAsync(authorizationGrant, cancellationToken);

        throw new NotImplementedException();
    }
}
