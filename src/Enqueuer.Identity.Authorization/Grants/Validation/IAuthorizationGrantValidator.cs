namespace Enqueuer.Identity.Authorization.Grants.Validation;

public interface IAuthorizationGrantValidator
{
    void Validate(IAuthorizationGrant grant);
}
