namespace Enqueuer.Identity.Authorization.Grants.Validation;

public interface IAuthorizationGrantValidator
{
    /// <summary>
    /// Validates the <paramref name="grant"/>.
    /// </summary>
    void Validate(IAuthorizationGrant grant);
}
