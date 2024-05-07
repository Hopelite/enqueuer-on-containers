using Microsoft.IdentityModel.Tokens;

namespace Enqueuer.Identity.Authorization.OAuth.Signature;

public interface ITokenSignatureProvider
{
    SigningCredentials GenerateSignature();
}
