using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Enqueuer.Identity.OAuth.JWT.Claims;
using Enqueuer.Identity.OAuth.Tokens;
using Microsoft.Extensions.Options;

namespace Enqueuer.Identity.OAuth.JWT;

public class JwtTokenGenerator : IAccessTokenGenerator
{
    private const string BearerTokenType = "bearer";
    private readonly JwtTokenConfiguration _tokenConfiguration;
    private readonly ISignatureProvider _signatureProvider;

    public JwtTokenGenerator(ISignatureProvider signatureProvider, IOptions<JwtTokenConfiguration> options)
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
            claims.Add(new ScopeClaim(context.Scope));
        }

        var signingCredentials = _signatureProvider.GetSigningCredentials();

        var token = new JwtSecurityTokenHandler().CreateJwtSecurityToken(
            issuer: _tokenConfiguration.Issuer,
            audience: context.Audience,
            subject: new ClaimsIdentity(claims),
            expires: DateTime.UtcNow.Add(_tokenConfiguration.TokenLifetime),
            signingCredentials: signingCredentials);

        return new AccessToken(token.ToString(), BearerTokenType, _tokenConfiguration.TokenLifetime);
    }
}
