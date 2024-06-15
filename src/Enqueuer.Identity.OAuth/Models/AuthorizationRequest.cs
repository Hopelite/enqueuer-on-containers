namespace Enqueuer.Identity.OAuth.Models;

public class AuthorizationRequest
{
    public AuthorizationRequest(string responseType, string clientId, Uri? redirectUri, Scope scope, string? state)
    {
        ResponseType = responseType;
        ClientId = clientId;
        RedirectUri = redirectUri;
        Scope = scope;
        State = state;
    }

    public string ResponseType { get; }

    public string ClientId { get; }

    public Uri? RedirectUri { get; }

    public Scope Scope { get; }

    public string? State { get; }
}