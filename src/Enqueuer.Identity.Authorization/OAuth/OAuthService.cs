using Enqueuer.Identity.Authorization.Configuration;
using Enqueuer.Identity.Authorization.Grants;
using Enqueuer.Identity.Authorization.Grants.Validation;
using Enqueuer.Identity.Authorization.Models;
using Enqueuer.Identity.Authorization.OAuth;
using Enqueuer.Identity.Authorization.OAuth.Signature;
using Enqueuer.Identity.Authorization.Validation;
using Microsoft.Extensions.Options;

namespace Enqueuer.Identity.Authorization;

public class OAuthService(
    IOptions<OAuthConfiguration> configuration,
    IAuthorizationGrantValidator grantValidator,
    IAuthorizationContext authorizationContext,
    ISignatureProviderFactory signatureProviderFactory, 
    IScopeValidator scopeValidator) : IOAuthService
{
    private readonly IOptions<OAuthConfiguration> _configuration = configuration;
    private readonly IAuthorizationGrantValidator _grantValidator = grantValidator;
    private readonly IAuthorizationContext _authorizationContext = authorizationContext;
    private readonly ISignatureProviderFactory _signatureProviderFactory = signatureProviderFactory;
    private readonly IScopeValidator _scopeValidator = scopeValidator;

    public async Task<AccessToken> GetAccessTokenAsync(IAuthorizationGrant grant, IReadOnlyCollection<Scope> scopes, CancellationToken cancellationToken)
    {
        _grantValidator.Validate(grant);
        await grant.AuthorizeAsync(_authorizationContext, cancellationToken);

        var signatureProvider = await _signatureProviderFactory.CreateAsync(cancellationToken);

        var tokenBuilder = AccessTokenBuilder.CreateBuilder(_configuration.Value, _scopeValidator)
            .AddScopes(scopes)
            .SignToken(signatureProvider);

        return tokenBuilder.Build();
    }
}
