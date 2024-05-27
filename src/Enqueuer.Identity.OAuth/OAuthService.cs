using Enqueuer.Identity.OAuth.Exceptions;
using Enqueuer.Identity.OAuth.Models;
using Enqueuer.Identity.OAuth.Storage;
using Enqueuer.Identity.OAuth.Validation;
using Enqueuer.Identity.OAuth.Validation.Grants;

namespace Enqueuer.Identity.OAuth;

// TODO: register as transient
public class OAuthService : IOAuthService
{
    private readonly IAuthorizationRequestValidator _requestValidator;
    private readonly IAuthorizationStorage _authorizationStorage;
    private readonly IAccessTokenManager _tokenManager;

    public OAuthService(
        IAuthorizationRequestValidator requestValidator,
        IAuthorizationStorage authorizationStorage,
        IAccessTokenManager tokenManager)
    {
        _requestValidator = requestValidator;
        _authorizationStorage = authorizationStorage;
        _tokenManager = tokenManager;
    }

    public async Task<AuthorizationResponse> AuthorizeAsync(AuthorizationRequest request, CancellationToken cancellationToken)
    {
        try
        {
            await _requestValidator.ValidateAsync(request, cancellationToken);

            var clientAuthorization = await _authorizationStorage.RegisterClientAuthorization(cancellationToken);

            // TODO: if the RedirectUri is null - use the one in the registration
            // If not - validate that it is one of the registered
            return new AuthorizationResponse(clientAuthorization.Code, request.State, request.RedirectUri);
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
            var token = await _tokenManager.GetAccessTokenAsync(request.AuthorizationGrant, cancellationToken);

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
