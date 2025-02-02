using System.Collections.Generic;

namespace Enqueuer.Identity.Persistence.Models;

/// <summary>
/// The unique role containing a set of related scopes.
/// </summary>
public class Role
{
    /// <summary>
    /// The unique identifier of the role.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The unique name of the role.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// A list of related to this role scopes.
    /// </summary>
    public IEnumerable<Scope> Scopes { get; set; } = new List<Scope>();

    /// <summary>
    /// A list of users and resources within which scope they have this role for access.
    /// </summary>
    public IEnumerable<UserResourceRoles> ResourceRoles { get; set; } = new List<UserResourceRoles>();
}
