using Enqueuer.Identity.Authorization.Extensions;

namespace Enqueuer.Identity.API.Services.Grants;

/// <summary>
/// The authorization grant used when applications request an access token to access their own resources, not on behalf of a user.
/// </summary>
public class ClientCredentialsGrant : IAuthorizationGrant
{
    public ClientCredentialsGrant(string clientId, string clientSecret)
    {
        ClientId = clientId.ThrowIfNullOrWhitespace(nameof(clientId));
        ClientSecret = clientSecret.ThrowIfNullOrWhitespace(nameof(clientSecret));
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
}
