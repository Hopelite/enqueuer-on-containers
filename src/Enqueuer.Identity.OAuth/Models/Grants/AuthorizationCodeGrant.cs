using Enqueuer.Identity.OAuth.Exceptions;
using Enqueuer.Identity.OAuth.Models.Enums;
using Enqueuer.Identity.OAuth.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace Enqueuer.Identity.OAuth.Models.Grants;

/// <summary>
/// The authorization grant used when an application exchanges an authorization code for an access token.
/// </summary>
public class AuthorizationCodeGrant : IAuthorizationGrant
{
    public AuthorizationCodeGrant(string code, Uri? redirectUri, string? clientId)
    {
        Code = ValidateCode(code);
        RedirectUri = redirectUri; // TODO: validate redirect_uri
        ClientId = ValidateClientId(clientId);
    }

    public string Type => AuthorizationGrantType.AuthorizationCode;

    /// <summary>
    /// The authorization code received from the authorization server.
    /// </summary>
    public string Code { get; }

    /// <summary>
    /// Required, if the redirect URI was included in the initial authorization request.
    /// </summary>
    public Uri? RedirectUri { get; }

    /// <summary>
    /// Required, if the client is not authenticating with the authorization server.
    /// </summary>
    /// <remarks>For now public clients are not supported.</remarks>
    public string? ClientId { get; }

    private static string ValidateCode(string code)
    {
        if (string.IsNullOrEmpty(code))
        {
            throw new InvalidGrantException("The 'code' query parameter is required.");
        }

        return code;
    }

    // TODO: possibly add support of the public clients in the future
    private static string ValidateClientId(string? clientId)
    {
        if (string.IsNullOrEmpty(clientId))
        {
            throw new InvalidClientException("The 'client_secret' query parameter is required.");
        }

        return clientId;
    }
}
