namespace Enqueuer.Identity.OAuth.Models;

/// <summary>
/// The response to successful authorization request containing the authorization code.
/// </summary>
public class AuthorizationResponse
{
    public AuthorizationResponse(in AuthorizationCode code, string? state)
    {
        Code = code;
        State = state;
    }

    /// <summary>
    /// The issued authorization code.
    /// </summary>
    public AuthorizationCode Code { get; }

    /// <summary>
    /// Not null if the "state" parameter was present in the client authorization request.
    /// The exact value received from the client.
    /// </summary>
    public string? State { get; }
}
