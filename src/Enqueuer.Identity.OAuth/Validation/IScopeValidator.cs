using Enqueuer.Identity.OAuth.Exceptions;
using Enqueuer.Identity.OAuth.Models;

namespace Enqueuer.Identity.OAuth.Validation;

public interface IScopeValidator
{
    /// <summary>
    /// Validates the <paramref name="scope"/> and throws an <see cref="AuthorizationException"/> if invalid.
    /// </summary>
    ValueTask ValidateScopeAsync(Scope scope, CancellationToken cancellationToken);
}
