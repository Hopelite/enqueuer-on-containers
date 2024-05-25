using Enqueuer.Identity.OAuth.Exceptions;

namespace Enqueuer.Identity.OAuth.Storage;

public interface IClientCredentialsStorage
{
    /// <summary>
    /// Gets the client secrent of the client with the specified <paramref name="clientId"/>.
    /// </summary>
    /// <exception cref="UnauthorizedClientException">Throws, if the specified <paramref name="clientId"/> is unknown.</exception>
    ValueTask<string> GetClientSecretAsync(string clientId, CancellationToken cancellationToken);

    // TODO: add client registration
}
