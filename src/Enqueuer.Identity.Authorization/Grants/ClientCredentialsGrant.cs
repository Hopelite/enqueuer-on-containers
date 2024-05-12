using Enqueuer.Identity.Authorization.Exceptions;
using Enqueuer.Identity.Authorization.Extensions;
using Enqueuer.Identity.Authorization.Grants.Credentials;
using Enqueuer.OAuth.Core.Tokens;
using Microsoft.Extensions.DependencyInjection;

namespace Enqueuer.Identity.Authorization.Grants;

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

    public string Type => AuthorizationGrantType.ClientCredentials.Type;

    /// <summary>
    /// The client identifier used to authenticate the client to the authorization server.
    /// </summary>
    public string ClientId { get; }

    /// <summary>
    /// The client secret used to authenticate the client to the authorization server.
    /// </summary>
    public string ClientSecret { get; }

    public async ValueTask AuthorizeAsync(IAuthorizationContext authorizationContext, CancellationToken cancellationToken)
    {
        var credentialStorage = authorizationContext.Services.GetRequiredService<IClientCredentialsStorage>();
        var actualClientSecret = await credentialStorage.GetClientSecretAsync(ClientId, cancellationToken);
        if (string.IsNullOrWhiteSpace(actualClientSecret))
        {
            throw new AuthorizationException("Specified client secret do not exist.");
        }

        if (!actualClientSecret.Equals(ClientSecret))
        {
            throw new AuthorizationException("Client secret is invalid.");
        }
    }
}
