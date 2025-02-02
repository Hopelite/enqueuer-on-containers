using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Enqueuer.Identity.OAuth.Tokens;
using Enqueuer.OAuth.Core.Claims;
using Enqueuer.OAuth.Core.Enums;
using Enqueuer.OAuth.Core.Tokens;
using Microsoft.Extensions.Options;

namespace Enqueuer.Identity.OAuth.JWT;

public class JwtTokenGenerator : IAccessTokenGenerator
{
    private readonly JwtTokenIssuingConfiguration _tokenConfiguration;
    private readonly ISignatureProvider _signatureProvider;

    public JwtTokenGenerator(ISignatureProvider signatureProvider, IOptions<JwtTokenIssuingConfiguration> options)
    {
        _signatureProvider = signatureProvider;
        _tokenConfiguration = options.Value;
    }

    public AccessToken GenerateToken(AccessTokenContext context)
    {
        var claims = new List<Claim>()
        {
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString(), ClaimValueTypes.String, _tokenConfiguration.Issuer)
        };

        if (context.Scope.HasValue)
        {
            claims.Add(new ScopeClaim(context.Scope, _tokenConfiguration.Issuer));
        }

        var signingCredentials = _signatureProvider.GetSigningCredentials();

        var tokenHandler = new JwtSecurityTokenHandler();

        var token = tokenHandler.CreateJwtSecurityToken(
            issuer: _tokenConfiguration.Issuer,
            audience: context.Audience,
            subject: new ClaimsIdentity(claims),
            expires: DateTime.UtcNow.Add(_tokenConfiguration.TokenLifetime),
            signingCredentials: signingCredentials);

        return new AccessToken(tokenHandler.WriteToken(token), TokenTypes.Bearer, _tokenConfiguration.TokenLifetime);
    }
}
