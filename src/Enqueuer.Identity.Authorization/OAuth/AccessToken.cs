namespace Enqueuer.Identity.Authorization.OAuth;

public class AccessToken
{
    internal AccessToken(string token, string type, TimeSpan expiresIn, string[]? scopes)
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
