using Microsoft.AspNetCore.Mvc;

namespace Enqueuer.Identity.API.Parameters;

/// <summary>
/// The request to create new or update the existing role.
/// </summary>
public class CreateOrUpdateRoleRequest
{
    /// <summary>
    /// The unique name of the role to create or update.
    /// </summary>
    [FromRoute(Name = "role_name")]
    public string RoleName { get; set; } = null!;

    /// <summary>
    /// The scopes assigned to this role. Can be empty for new roles or to remove all assigned to the existing one.
    /// </summary>
    [FromBody]
    public IReadOnlyCollection<Scope> Scopes { get; set; } = Array.Empty<Scope>();
}

/// <summary>
/// The new or existing scope to assign.
/// </summary>
public class Scope
{
    /// <summary>
    /// The unique name of the new or existing scope.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// The child scopes of this scope if they exist.
    /// </summary>
    public IReadOnlyCollection<Scope>? ChildScopes { get; set; }
}
