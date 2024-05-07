using Enqueuer.Identity.Authorization.OAuth.Signature;
using Microsoft.IdentityModel.Tokens;

namespace Enqueuer.Identity.API.Services;

public class TokenSignatureProvider(TokenSignatureProviderConfiguration configuration) : ITokenSignatureProvider
{
    private readonly TokenSignatureProviderConfiguration _configuration = configuration;

    public SigningCredentials GenerateSignature()
    {
        var key = new SymmetricSecurityKey(_configuration.Key);
        return new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
    }
}
