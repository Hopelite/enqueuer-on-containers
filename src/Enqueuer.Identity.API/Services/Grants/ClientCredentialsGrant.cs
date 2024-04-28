using Enqueuer.Identity.API.Extensions;

namespace Enqueuer.Identity.API.Services.Grants;

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
