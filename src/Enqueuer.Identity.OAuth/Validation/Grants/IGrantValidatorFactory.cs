namespace Enqueuer.Identity.OAuth.Validation.Grants;

public interface IGrantValidatorFactory
{
    IGrantValidator GetGrantValidator(string grantType);
}
