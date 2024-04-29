using Enqueuer.Identity.Authorization.Models;

namespace Enqueuer.Identity.Authorization;

public interface IAuthorizationService
{
    /// <summary>
    /// Grants access defined by the <paramref name="role"/> scopes to resource with the specified <paramref name="resourceUri"/>
    /// for user with the specified <paramref name="granteeId"/>.
    /// </summary>
    /// <remarks>User can only have one role withing scope of the resource.</remarks>
    Task GrantAccessAsync(Uri resourceUri, long granteeId, Role role, CancellationToken cancellationToken);

    /// <summary>
    /// Revokes access to resource with the specified <paramref name="resourceUri"/> from user with the specified <paramref name="granteeId"/>.
    /// </summary>
    Task RevokeAccessAsync(Uri resourceUri, long granteeId, CancellationToken cancellationToken);
}
