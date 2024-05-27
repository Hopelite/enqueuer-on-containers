using Enqueuer.Identity.OAuth.Exceptions;
using Enqueuer.Identity.OAuth.Models;

namespace Enqueuer.Identity.OAuth.Storage;

public interface IAuthorizationStorage
{
    /// <summary>
    /// Generates the authorization code and stores it in cache.
    /// </summary>
    Task<ClientAuthorization> RegisterClientAuthorization(CancellationToken cancellationToken);

    /// <summary>
    /// 
    /// </summary>
    /// <exception cref="InvalidClientException">Thrown, if the client authorization does not exist.</exception>
    Task CheckClientAuthorizationAsync(ClientAuthorization clientAuthorization, CancellationToken cancellationToken);
}
