namespace Enqueuer.Identity.API.Services.Grants;

public interface IAuthorizationGrantValidator
{
    void Validate(IAuthorizationGrant grant);
}
