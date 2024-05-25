using Enqueuer.Identity.OAuth.Models;
using Enqueuer.Identity.OAuth.Storage;
using Enqueuer.Identity.OAuth.Validation;

namespace Enqueuer.Identity.OAuth;

public class OAuthService : IOAuthService
{
    private readonly IAuthorizationRequestValidator _requestValidator;
    private readonly IAuthorizationStorage _authorizationStorage;

    public OAuthService(
        IAuthorizationRequestValidator requestValidator,
        IAuthorizationStorage authorizationStorage)
    {
        _requestValidator = requestValidator;
        _authorizationStorage = authorizationStorage;
    }

    public async Task<AuthorizationResponse> AuthorizeAsync(AuthorizationRequest request, CancellationToken cancellationToken)
    {
        await _requestValidator.ValidateAsync(request, cancellationToken);

        var code = await _authorizationStorage.RequestCodeAsync(cancellationToken);

        return new AuthorizationResponse(code, request.State);
    }
}
