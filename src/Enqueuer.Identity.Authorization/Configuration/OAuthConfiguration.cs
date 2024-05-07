namespace Enqueuer.Identity.Authorization.Configuration;

public class OAuthConfiguration
{
    public OAuthConfiguration()
    {
        
    }

    public OAuthConfiguration(string issuer, int tokenLifetime)
    {
        Issuer = issuer;
        TokenLifetime = tokenLifetime;
    }

    public string Issuer { get; init; }

    public int TokenLifetime { get; init; }
}
