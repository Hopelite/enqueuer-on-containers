using Enqueuer.OAuth.Core.Exceptions;

namespace Enqueuer.Identity.OAuth.Storage;

public interface IClientCredentialsStorage
{
    /// <summary>
    /// Authorizes the provided <paramref name="clientId"/> and <paramref name="clientSecret"/> pair.
    /// </summary>
    /// <exception cref="UnauthorizedClientException">Thrown, if provided credentials are invalid.</exception>
    Task AuthorizeClientAsync(string clientId, string clientSecret, CancellationToken cancellationToken);
}
