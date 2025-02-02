using Enqueuer.Identity.OAuth.Storage;

namespace Enqueuer.Identity.API.Services;

public class KeyVaultStab : IClientCredentialsStorage
{
    public Task AuthorizeClientAsync(string clientId, string clientSecret, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}