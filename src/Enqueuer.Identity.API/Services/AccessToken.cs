namespace Enqueuer.Identity.API.Services;

public class AccessToken
{
    public AccessToken(string token, string type, TimeSpan expiresIn)
        : this(token, type, expiresIn, null)
    {
    }

    public AccessToken(string token, string type, TimeSpan expiresIn, string[]? scopes)
    {
        Token = token;
        Type = type;
        ExpiresIn = expiresIn;
        Scopes = scopes;
    }

    public string Token { get; }

    public string Type { get; }

    public TimeSpan ExpiresIn { get; }

    public string[]? Scopes { get; }
}
