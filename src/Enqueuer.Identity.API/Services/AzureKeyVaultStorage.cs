using Azure;
using Azure.Security.KeyVault.Secrets;
using Enqueuer.Identity.OAuth.Storage;
using Enqueuer.OAuth.Core.Exceptions;

namespace Enqueuer.Identity.API.Services;

public class AzureKeyVaultStorage(SecretClient secretClient) : IClientCredentialsStorage
{
    private readonly SecretClient _secretClient = secretClient;

    public async Task AuthorizeClientAsync(string clientId, string clientSecret, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(clientId))
        {
            throw new InvalidClientException("Missing the 'client_id' parameter.");
        }

        if (string.IsNullOrWhiteSpace(clientSecret))
        {
            throw new InvalidClientException("Missing the 'client_secret' parameter.");
        }

        try
        {
            var existingClientSecret = (await _secretClient.GetSecretAsync(clientId, cancellationToken: cancellationToken)).Value;
            if (!string.Equals(existingClientSecret.Value, clientSecret))
            {
                throw new InvalidClientException();
            }
        }
        catch (RequestFailedException ex) when (ex.ErrorCode == "SecretNotFound")
        {
            throw new InvalidClientException();
        }
    }
}
