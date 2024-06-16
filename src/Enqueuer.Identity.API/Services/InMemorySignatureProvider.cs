using Enqueuer.Identity.OAuth.JWT;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Enqueuer.Identity.API.Services;

public class InMemorySignatureProvider(IOptions<OAuthConfiguration> options) : ISignatureProvider
{
    private readonly SecurityKey _signatureKey = options.Value.GetSigningKey();

    public SigningCredentials GetSigningCredentials()
    {
        return new SigningCredentials(_signatureKey, SecurityAlgorithms.HmacSha256);
    }
}
