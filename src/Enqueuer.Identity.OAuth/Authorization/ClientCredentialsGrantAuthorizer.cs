using Enqueuer.Identity.OAuth.Models.Grants;
using Enqueuer.Identity.OAuth.Storage;
using Enqueuer.Identity.OAuth.Tokens;

namespace Enqueuer.Identity.OAuth.Authorization;

public class ClientCredentialsGrantAuthorizer : GrantAuthorizerBase<ClientCredentialsGrant>
{
    private readonly IClientCredentialsStorage _credentialsStorage;

    public ClientCredentialsGrantAuthorizer(IClientCredentialsStorage credentialsStorage)
    {
        _credentialsStorage = credentialsStorage;
    }

    protected override async Task<AccessTokenContext> AuthorizeGrantAsync(ClientCredentialsGrant grant, CancellationToken cancellationToken)
    {
        await _credentialsStorage.AuthorizeClientAsync(grant.ClientId, grant.ClientSecret, cancellationToken);
        return new AccessTokenContext(grant.ClientId, grant.Scope);
    }
}
