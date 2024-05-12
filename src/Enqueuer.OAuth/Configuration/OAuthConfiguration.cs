using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Microsoft.Extensions.Configuration;

public class OAuthConfiguration
{
    private const int DefaultTokenLifetime = 3600; 
    private string _signingKey = default!;
    private int _expiresInSeconds = DefaultTokenLifetime;

    public string? Audience { get; init; }

    public string Issuer { get; init; } = default!;

    public string SigningKey
    {
        get => _signingKey;
        set
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(nameof(value));
            _signingKey = value;
        }
    }

    public int TokenLifetimeInSeconds
    {
        get => _expiresInSeconds;
        set
        {
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Token lifetime in seconds can't be negative or equal zero.");
            }

            _expiresInSeconds = value;
        }
    }

    public TimeSpan TokenLifetime => TimeSpan.FromSeconds(TokenLifetimeInSeconds);

    public SecurityKey GetSigningKey() => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_signingKey));
}
