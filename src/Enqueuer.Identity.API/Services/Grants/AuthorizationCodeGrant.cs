namespace Enqueuer.Identity.API.Services.Grants;

public class AuthorizationCodeGrant : IAuthorizationGrant
{
    public AuthorizationCodeGrant(string code)
    {
        Code = code;
    }

    public string Type => AuthorizationGrantType.AuthorizationCode;

    public string Code { get; }
}
