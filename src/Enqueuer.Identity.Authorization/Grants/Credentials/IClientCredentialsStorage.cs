namespace Enqueuer.Identity.Authorization.Grants.Credentials;

public interface IClientCredentialsStorage
{
    ValueTask<string?> GetClientSecretAsync(string clientId, CancellationToken cancellationToken);
}
