using Microsoft.IdentityModel.Tokens;

namespace Enqueuer.Identity.OAuth.JWT;

public interface ISignatureProvider
{
    SigningCredentials GetSigningCredentials();
}
