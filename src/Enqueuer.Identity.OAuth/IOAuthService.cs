using Enqueuer.Identity.OAuth.Models;

namespace Enqueuer.Identity.OAuth;

public interface IOAuthService
{
    /// <summary>
    /// Authorizes the <paramref name="request"/> and returns <see cref="AuthorizationResponse"/> with the <see cref="AuthorizationCode"/> if successful.
    /// </summary>
    Task<AuthorizationResponse> AuthorizeAsync(AuthorizationRequest request, CancellationToken cancellationToken);
}
