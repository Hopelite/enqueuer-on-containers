using Enqueuer.Identity.Authorization.OAuth.Signature;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace Enqueuer.Identity.API.Services;

public class InMemorySignatureProvider(SecurityKey signatureKey) : ITokenSignatureProvider
{
    private readonly SecurityKey _signatureKey = signatureKey;

    public Task<string> SignAsync(JwtSecurityToken token, CancellationToken cancellationToken)
    {
        // TODO: consider to not recreate the token 
        ArgumentNullException.ThrowIfNull(token, nameof(token));

        var signature = new SigningCredentials(_signatureKey, SecurityAlgorithms.HmacSha256);

        var signedToken = new JwtSecurityToken(
            issuer: token.Issuer,
            claims: token.Claims,
            expires: token.ValidTo,
            signingCredentials: signature);

        var handler = new JwtSecurityTokenHandler();
        return Task.FromResult(handler.WriteToken(signedToken));
    }
}
