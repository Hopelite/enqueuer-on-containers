using Enqueuer.OAuth.Core.Models;

namespace Enqueuer.Identity.OAuth.Tokens;

/// <summary>
/// Containing neccessary data for access token generation.
/// </summary>
public class AccessTokenContext
{
    public AccessTokenContext(string audience, Scope scope)
    {
        Audience = audience;
        Scope = scope;
    }

    public string Audience { get; }

    public Scope Scope { get; }
}
