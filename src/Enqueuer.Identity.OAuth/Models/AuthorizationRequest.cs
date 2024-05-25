using Enqueuer.Identity.OAuth.Exceptions;
using Enqueuer.Identity.OAuth.Extensions;
using Enqueuer.Identity.OAuth.Models.Enums;

namespace Enqueuer.Identity.OAuth.Models;

public class AuthorizationRequest
{
    public AuthorizationRequest(string responseType, string clientId, Uri? redirectUri, Scope scope, string? state)
    {
        // TODO: possibly throw more specific authorization exceptions
        ResponseType = ValidateResponseType(responseType);
        ClientId = clientId.ThrowIfNullOrWhitespace(nameof(clientId));
        RedirectUri = ValidateRedirectUri(redirectUri);
        Scope = ValidateScope(scope);
        State = state;
    }

    public string ResponseType { get; }

    public string ClientId { get; }

    public Uri? RedirectUri { get; }

    public Scope Scope { get; }

    public string? State { get; }

    // TODO: move validation to separate class
    private static string ValidateResponseType(string responseType)
    {
        if (string.IsNullOrWhiteSpace(responseType))
        {
            throw new InvalidRequestException("The response_type parameter is required.");
        }

        if (responseType != Enums.ResponseType.AuthorizationCode)
        {
            throw new UnsupportedResponseTypeException($"The response_type '{responseType}' is unsupported");
        }

        return responseType;
    }

    private static Uri? ValidateRedirectUri(Uri? redirectUri)
    {
        if (redirectUri != null && !redirectUri.IsAbsoluteUri)
        {
            throw new InvalidRequestException("The redirect_uri parameter must contain an absolute URL.");
        }

        return redirectUri;
    }

    private static string[] SupportedScopes = [
        "queue",
        // Get from database
    ];

    private static Scope ValidateScope(Scope scope)
    {
        if (scope == null)
        {
            throw new ServerErrorException(new ArgumentNullException(nameof(scope)));
        }

        foreach (var scopeValue in scope)
        {
            if (!SupportedScopes.Contains(scopeValue))
            {
                throw InvalidScopeException.FromScope(scopeValue);
            }
        }

        return scope;
    }
}
