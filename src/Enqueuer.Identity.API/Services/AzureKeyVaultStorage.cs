using Azure.Security.KeyVault.Secrets;
using Enqueuer.Identity.Authorization.Grants.Credentials;

namespace Enqueuer.Identity.API.Services;

public class AzureKeyVaultStorage(SecretClient secretClient) : IClientCredentialsStorage
{
    private readonly SecretClient _secretClient = secretClient;

    public async ValueTask<string> GetClientSecretAsync(string clientId, CancellationToken cancellationToken)
    {
        var clientSecret = (await _secretClient.GetSecretAsync(clientId, cancellationToken: cancellationToken)).Value;
        return clientSecret.Value;
    }
}
