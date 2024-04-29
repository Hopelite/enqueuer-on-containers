using System.Collections.Generic;

namespace Enqueuer.Identity.Persistence.Models;

/// <summary>
/// The unique scope which represents possible permissions.
/// </summary>
public class Scope
{
    /// <summary>
    /// The unique identifier of the scope.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The unique name of the scope.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// The unique identifier of the parent scope, if the scope is not located at the top of hierarchy.
    /// </summary>
    /// <example>The "queue:delete" scope will have the "queue" scope ID set to ParentId.</example>
    public int? ParentId { get; set; }

    /// <summary>
    /// The parent scope, if the scope is not located at the top of hierarchy.
    /// </summary>
    public Scope? Parent { get; set; }

    /// <summary>
    /// A list of child scopes directly derived from this scope.
    /// </summary>
    public IEnumerable<Scope> Children { get; set; } = new List<Scope>();

    /// <summary>
    /// A list of roles that have this scope.
    /// </summary>
    public IEnumerable<Role> Roles { get; set; } = new List<Role>();
}
