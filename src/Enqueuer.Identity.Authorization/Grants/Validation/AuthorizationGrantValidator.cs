namespace Enqueuer.Identity.Authorization.Grants.Validation;

public class AuthorizationGrantValidator : IAuthorizationGrantValidator
{
    private static readonly string[] SupportedGrantTypes =
    [
        AuthorizationGrantType.ClientCredentials,
    ];

    public void Validate(IAuthorizationGrant grant)
    {
        if (!SupportedGrantTypes.Contains(grant.Type))
        {
            throw new UsupportedGrantTypeException(grant.Type, $"The grant type \"{grant.Type}\" is unsupported.");
        }

        // TODO: authenticate grant by type
    }
}
