namespace Enqueuer.Identity.OAuth.Models;

public readonly struct ClientAuthorization
{
    internal ClientAuthorization(string code, Uri? redirectUri, string clientId)
        : this(new AuthorizationCode(code), redirectUri, clientId)
    {
    }

    internal ClientAuthorization(AuthorizationCode code, Uri? redirectUri, string clientId)
    {
        Code = code;
        RedirectUri = redirectUri;
        ClientId = clientId;
    }

    public AuthorizationCode Code { get; }

    /// <summary>
    /// 
    /// </summary>
    public Uri? RedirectUri { get; }

    // TODO: consider to be nullable for public clients
    public string ClientId { get; }
}
