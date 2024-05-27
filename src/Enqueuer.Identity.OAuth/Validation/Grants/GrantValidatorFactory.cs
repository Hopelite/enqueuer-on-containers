using Enqueuer.Identity.OAuth.Exceptions;
using Enqueuer.Identity.OAuth.Models.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace Enqueuer.Identity.OAuth.Validation.Grants;

public class GrantValidatorFactory : IGrantValidatorFactory
{
    private readonly IServiceProvider _serviceProvider;

    public GrantValidatorFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IGrantValidator GetGrantValidator(string grantType)
    {
        return grantType switch
        {
            AuthorizationGrantType.AuthorizationCode => _serviceProvider.GetRequiredService<AuthorizationCodeGrantValidator>(),
            AuthorizationGrantType.ClientCredentials => _serviceProvider.GetRequiredService<ClientCredentialsGrantValidator>(),
            _ => throw UnsupportedGrantTypeException.FromGrantType(grantType)
        };
    }
}
