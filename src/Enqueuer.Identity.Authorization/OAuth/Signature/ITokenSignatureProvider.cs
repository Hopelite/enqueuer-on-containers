using System.IdentityModel.Tokens.Jwt;

namespace Enqueuer.Identity.Authorization.OAuth.Signature;

public interface ITokenSignatureProvider
{
    Task<string> SignAsync(JwtSecurityToken token, CancellationToken cancellationToken);
}
