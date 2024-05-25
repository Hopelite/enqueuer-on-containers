using Enqueuer.Identity.OAuth.Exceptions;
using Enqueuer.Identity.OAuth.Models;
using Enqueuer.Identity.OAuth.Storage;
using Enqueuer.Identity.OAuth.Validation;

namespace Enqueuer.Identity.OAuth;

// TODO: register as transient
public class OAuthService : IOAuthService
{
    private readonly IAuthorizationRequestValidator _requestValidator;
    private readonly IAuthorizationStorage _authorizationStorage;
    private readonly IAuthorizationContext _authorizationContext;

    public OAuthService(
        IAuthorizationRequestValidator requestValidator,
        IAuthorizationStorage authorizationStorage,
        IAuthorizationContext authorizationContext)
    {
        _requestValidator = requestValidator;
        _authorizationStorage = authorizationStorage;
        _authorizationContext = authorizationContext;
    }

    public async Task<AuthorizationResponse> AuthorizeAsync(AuthorizationRequest request, CancellationToken cancellationToken)
    {
        try
        {
            await _requestValidator.ValidateAsync(request, cancellationToken);

            var code = await _authorizationStorage.RequestCodeAsync(cancellationToken);

            // TODO: if the RedirectUri is null - use the one in the registration
            // If not - validate that it is one of the registered
            return new AuthorizationResponse(code, request.State, request.RedirectUri);
        }
        catch (AuthorizationException)
        {
            throw;
        }
        catch (Exception ex)
        {
            // All internal exceptions must be consealed from the client
            throw new ServerErrorException(ex);
        }
    }

    public async Task<AccessTokenResponse> GetAccessTokenAsync(AccessTokenRequest request, CancellationToken cancellationToken)
    {
        try
        {
            await request.AuthorizationGrant.AuthorizeAsync(_authorizationContext, cancellationToken);

            var token = new AccessToken(); // TODO: implement access token generation

            return new AccessTokenResponse(token);
        }
        catch (AuthorizationException)
        {
            throw;
        }
        catch (Exception ex)
        {
            // All internal exceptions must be consealed from the client
            throw new ServerErrorException(ex);
        }
    }
}
