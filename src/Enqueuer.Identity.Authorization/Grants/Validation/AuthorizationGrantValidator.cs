namespace Enqueuer.Identity.Authorization.Grants.Validation;

public class AuthorizationGrantValidator : IAuthorizationGrantValidator
{
    private static readonly string[] SupportedGrantTypes =
    [
        AuthorizationGrantType.ClientCredentials,
    ];

    /// <inheritdoc/>
    /// <exception cref="UsupportedGrantTypeException"/>
    public void Validate(IAuthorizationGrant grant)
    {
        if (!SupportedGrantTypes.Contains(grant.Type))
        {
            throw new UsupportedGrantTypeException(grant.Type, $"The grant type \"{grant.Type}\" is unsupported.");
        }
    }
}
