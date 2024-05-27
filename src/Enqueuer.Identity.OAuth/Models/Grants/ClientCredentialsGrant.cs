using Enqueuer.Identity.OAuth.Models.Enums;

namespace Enqueuer.Identity.OAuth.Models.Grants;

/// <summary>
/// The authorization grant used when applications request an access token to access their own resources, not on behalf of a user.
/// </summary>
public class ClientCredentialsGrant : IAuthorizationGrant
{
    public ClientCredentialsGrant(string clientId, string clientSecret, Scope scope)
    {
        ClientId = clientId;
        ClientSecret = clientSecret;
        Scope = scope;
    }

    public string Type => AuthorizationGrantType.ClientCredentials;

    /// <summary>
    /// The client identifier used to authenticate the client to the authorization server.
    /// </summary>
    public string ClientId { get; }

    /// <summary>
    /// The client secret used to authenticate the client to the authorization server.
    /// </summary>
    public string ClientSecret { get; }

    /// <summary>
    /// The scope of the access request.
    /// </summary>
    public Scope Scope { get; }
}
