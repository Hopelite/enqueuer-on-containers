using Enqueuer.Identity.Authorization.Exceptions;

namespace Enqueuer.Identity.Authorization.Grants;

/// <summary>
/// Defines a credential representing the resource owner's authorization to the client to obtain an access token.
/// </summary>
public interface IAuthorizationGrant
{
    /// <summary>
    /// The type of the authorization grant.
    /// </summary>
    string Type { get; }

    /// <summary>
    /// Authorizes this grant using the <paramref name="authorizationContext"/>.
    /// </summary>
    /// <exception cref="AuthorizationException">Thrown in case the grant cannot be authorized with.</exception>
    ValueTask AuthorizeAsync(IAuthorizationContext authorizationContext, CancellationToken cancellationToken);
}
