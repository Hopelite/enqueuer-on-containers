using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.Extensions.Configuration;

/// <summary>
/// Contains OAuth configuration for resource servers.
/// </summary>
public class OAuthConfiguration
{
    private string _signingKey = default!;

    public required string Issuer { get; init; }

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
