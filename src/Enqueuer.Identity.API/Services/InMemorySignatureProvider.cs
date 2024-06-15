using Enqueuer.Identity.OAuth.JWT;
using Microsoft.IdentityModel.Tokens;

namespace Enqueuer.Identity.API.Services;

public class InMemorySignatureProvider(SecurityKey signatureKey) : ISignatureProvider
{
    private readonly SecurityKey _signatureKey = signatureKey;

    public SigningCredentials GetSigningCredentials()
    {
        return new SigningCredentials(_signatureKey, SecurityAlgorithms.HmacSha256);
    }
}
