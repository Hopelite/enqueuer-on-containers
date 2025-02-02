using Enqueuer.OAuth.Core.Enums;
using Enqueuer.OAuth.Core.Exceptions;
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
        if (string.IsNullOrWhiteSpace(grantType))
        {
            throw new InvalidRequestException("The 'grant_type' parameter is required.");
        }

        return grantType switch
        {
            AuthorizationGrantType.ClientCredentials.Type => _serviceProvider.GetRequiredService<ClientCredentialsGrantAuthorizer>(),
            _ => throw UnsupportedGrantTypeException.FromGrantType(grantType)
        };
    }
}
