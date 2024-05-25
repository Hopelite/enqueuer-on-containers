using Enqueuer.Identity.OAuth.Exceptions;
using Enqueuer.Identity.OAuth.Models;

namespace Enqueuer.Identity.OAuth.Validation;

public interface IAuthorizationRequestValidator
{
    /// <summary>
    /// Validates the <paramref name="request"/> and throws an <see cref="AuthorizationException"/> if invalid.
    /// </summary>
    ValueTask ValidateAsync(AuthorizationRequest request, CancellationToken cancellationToken);
}
