using Azure;
using Azure.Security.KeyVault.Secrets;
using Enqueuer.Identity.OAuth.Exceptions;
using Enqueuer.Identity.OAuth.Storage;

namespace Enqueuer.Identity.API.Services;

public class AzureKeyVaultStorage(SecretClient secretClient) : IClientCredentialsStorage
{
    private readonly SecretClient _secretClient = secretClient;

    public async Task AuthorizeClientAsync(string clientId, string clientSecret, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(nameof(clientId));
        ArgumentNullException.ThrowIfNull(nameof(clientSecret));

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
