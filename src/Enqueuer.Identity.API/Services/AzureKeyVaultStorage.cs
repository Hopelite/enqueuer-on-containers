using Azure;
using Azure.Security.KeyVault.Secrets;
using Enqueuer.Identity.OAuth.Exceptions;
using Enqueuer.Identity.OAuth.Storage;

namespace Enqueuer.Identity.API.Services;

public class AzureKeyVaultStorage(SecretClient secretClient) : IClientCredentialsStorage
{
    private readonly SecretClient _secretClient = secretClient;

    public async ValueTask<string> GetClientSecretAsync(string clientId, CancellationToken cancellationToken)
    {
        try
        {
            var clientSecret = (await _secretClient.GetSecretAsync(clientId, cancellationToken: cancellationToken)).Value;
            return clientSecret.Value;
        }
        catch (RequestFailedException ex) when (ex.ErrorCode == "SecretNotFound")
        {
            throw new UnauthorizedClientException();
        }
    }
}
