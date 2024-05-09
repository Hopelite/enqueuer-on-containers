using Enqueuer.Identity.Authorization.OAuth.Signature;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace Enqueuer.Identity.API.Services;

public class InMemorySignatureProvider(TokenSignatureProviderConfiguration configuration) : ITokenSignatureProvider
{
    private readonly TokenSignatureProviderConfiguration _configuration = configuration;

    public Task<string> SignAsync(JwtSecurityToken token, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(token, nameof(token));

        var signatureKey = new SymmetricSecurityKey(_configuration.EncodedKey);
        var signature = new SigningCredentials(signatureKey, SecurityAlgorithms.HmacSha256);

        var signedToken = new JwtSecurityToken(
            issuer: token.Issuer,
            claims: token.Claims,
            expires: token.ValidTo,
            signingCredentials: signature);

        var handler = new JwtSecurityTokenHandler();
        return Task.FromResult(handler.WriteToken(signedToken));
    }
}
