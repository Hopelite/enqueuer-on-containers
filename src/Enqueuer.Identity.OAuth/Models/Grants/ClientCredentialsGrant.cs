using Enqueuer.Identity.OAuth.Exceptions;
using Enqueuer.Identity.OAuth.Models.Enums;
using Enqueuer.Identity.OAuth.Storage;
using Microsoft.Extensions.DependencyInjection;

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

    public async ValueTask AuthorizeAsync(IAuthorizationContext authorizationContext, CancellationToken cancellationToken)
    {
        var credentialStorage = authorizationContext.Services.GetRequiredService<IClientCredentialsStorage>();
        var actualClientSecret = await credentialStorage.GetClientSecretAsync(ClientId, cancellationToken);

        if (string.IsNullOrWhiteSpace(actualClientSecret) || !actualClientSecret.Equals(ClientSecret))
        {
            throw new InvalidClientException("The value of the client_secret parameter is invalid.");
        }
    }
}
