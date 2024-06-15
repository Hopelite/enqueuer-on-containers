using Enqueuer.Identity.OAuth.Exceptions;
using Enqueuer.Identity.OAuth.Models.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace Enqueuer.Identity.OAuth.Authorization;

public class GrantAuthorizerFactory : IGrantAuthorizerFactory
{
    private readonly IServiceProvider _serviceProvider;

    public GrantAuthorizerFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IGrantAuthorizer GetAuthorizerFor(string grantType)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(grantType);

        return grantType switch
        {
            AuthorizationGrantType.ClientCredentials => _serviceProvider.GetRequiredService<ClientCredentialsGrantAuthorizer>(),
            _ => throw UnsupportedGrantTypeException.FromGrantType(grantType)
        };
    }
}
