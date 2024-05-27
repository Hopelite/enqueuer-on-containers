using Enqueuer.Identity.OAuth.Exceptions;
using Enqueuer.Identity.OAuth.Models.Grants;

namespace Enqueuer.Identity.OAuth.Validation.Grants;

public interface IGrantValidator
{
    /// <summary>
    /// Authorizes the <paramref name="grant"/>.
    /// </summary>
    /// <exception cref="AuthorizationException">Thrown in case the grant cannot be authorized with.</exception>
    Task AuthorizeAsync(IAuthorizationGrant grant, CancellationToken cancellationToken);
}
