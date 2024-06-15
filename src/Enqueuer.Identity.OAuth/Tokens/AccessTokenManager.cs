using Enqueuer.Identity.OAuth.Authorization;
using Enqueuer.Identity.OAuth.Models.Grants;
using Enqueuer.Identity.OAuth.Storage;

namespace Enqueuer.Identity.OAuth.Tokens;

public class AccessTokenManager : IAccessTokenManager
{
    private readonly IGrantAuthorizerFactory _authorizerFactory;
    private readonly IAccessTokenGenerator _tokenGenerator;
    private readonly IAccessTokenStorage _tokenStorage;

    public AccessTokenManager(IGrantAuthorizerFactory authorizerFactory, IAccessTokenGenerator tokenGenerator, IAccessTokenStorage tokenStorage)
    {
        _authorizerFactory = authorizerFactory;
        _tokenGenerator = tokenGenerator;
        _tokenStorage = tokenStorage;
    }

    public async Task<AccessToken> RequestAccessTokenAsync(IAuthorizationGrant authorizationGrant, CancellationToken cancellationToken)
    {
        var grantAuthorizer = _authorizerFactory.GetAuthorizerFor(authorizationGrant.Type);
        var tokenContext = await grantAuthorizer.AuthorizeAsync(authorizationGrant, cancellationToken);

        var token = _tokenGenerator.GenerateToken(tokenContext);
        await _tokenStorage.SaveAccessTokenAsync(token, cancellationToken);

        return token;
    }
}
