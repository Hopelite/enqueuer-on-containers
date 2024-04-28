namespace Enqueuer.Identity.API.Services;

public class AccessToken
{
    public AccessToken(string token, string type, TimeSpan expiresIn)
    {
        Token = token;
        Type = type;
        ExpiresIn = expiresIn;
    }

    public string Token { get; }

    public string Type { get; }

    public TimeSpan ExpiresIn { get; }
}
