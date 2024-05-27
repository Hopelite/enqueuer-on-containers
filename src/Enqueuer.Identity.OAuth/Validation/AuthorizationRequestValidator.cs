using Enqueuer.Identity.OAuth.Exceptions;
using Enqueuer.Identity.OAuth.Models;
using Enqueuer.Identity.OAuth.Models.Enums;
using Enqueuer.Identity.OAuth.Storage;

namespace Enqueuer.Identity.OAuth.Validation;

public class AuthorizationRequestValidator : IAuthorizationRequestValidator
{
    private readonly IClientCredentialsStorage _clientSecretStorage;
    private readonly IScopeValidator _scopeValidator;

    public AuthorizationRequestValidator(IClientCredentialsStorage clientValidator, IScopeValidator scopeValidator)
    {
        _clientSecretStorage = clientValidator;
        _scopeValidator = scopeValidator;
    }

    public async ValueTask ValidateAsync(AuthorizationRequest request, CancellationToken cancellationToken)
    {
        ValidateResponseType(request.ResponseType);
        ValidateRedirectUri(request.RedirectUri);

        await ValidateClientIdAsync(request.ClientId, cancellationToken);
        await _scopeValidator.ValidateScopeAsync(request.Scope, cancellationToken);
    }

    private static void ValidateResponseType(string responseType)
    {
        if (string.IsNullOrWhiteSpace(responseType))
        {
            throw new InvalidRequestException("The 'response_type' query parameter is required.");
        }

        if (responseType != ResponseType.AuthorizationCode)
        {
            throw new UnsupportedResponseTypeException($"The response_type '{responseType}' is unsupported");
        }
    }

    private static void ValidateRedirectUri(Uri? redirectUri)
    {
        if (redirectUri != null && !redirectUri.IsAbsoluteUri)
        {
            throw new InvalidRequestException("The 'redirect_uri' query parameter must contain an absolute URL.");
        }
    }

    private async ValueTask ValidateClientIdAsync(string clientId, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(clientId))
        {
            throw new InvalidRequestException("The 'client_id' query parameter is required.");
        }

        await _clientSecretStorage.GetClientSecretAsync(clientId, cancellationToken);
    }
}
