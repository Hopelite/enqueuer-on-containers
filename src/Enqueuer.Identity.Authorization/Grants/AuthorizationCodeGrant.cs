
using Enqueuer.OAuth.Core.Tokens;

namespace Enqueuer.Identity.Authorization.Grants;

/// <summary>
/// The authorization grant used when an application exchanges an authorization code for an access token.
/// </summary>
public class AuthorizationCodeGrant : IAuthorizationGrant
{
    public AuthorizationCodeGrant(string code, string? redirectUri, string? clientId)
    {
        Code = code;
        RedirectUri = redirectUri == null ? null : new Uri(redirectUri);
        ClientId = clientId;
    }

    public AuthorizationCodeGrant(string code, Uri? redirectUri, string? clientId)
    {
        Code = code;
        RedirectUri = redirectUri;
        ClientId = clientId;
    }

    public string Type => AuthorizationGrantType.AuthorizationCode.Type;

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
    public string? ClientId { get; }

    public ValueTask AuthorizeAsync(IAuthorizationContext authorizationContext, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
