using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Microsoft.Extensions.Configuration;

public class OAuthConfiguration
{
    private string _signingKey = default!;

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

    public SecurityKey GetSigningKey() => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_signingKey));
}
