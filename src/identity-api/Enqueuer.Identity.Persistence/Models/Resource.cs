using System;
using System.Collections.Generic;

namespace Enqueuer.Identity.Persistence.Models;

/// <summary>
/// The unique resource 
/// </summary>
public class Resource
{
    /// <summary>
    /// The unique identifier of the resource within the database.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The unique resource identifier.
    /// </summary>
    public Uri Uri { get; set; }

    /// <summary>
    /// The unique identifier of the parent resource, if the resource is not located at the top of hierarchy.
    /// </summary>
    /// <example>The "groups/1/queues/TestQueue" resource will have the "groups/1/queues" scope ID set to ParentId.</example>
    public int? ParentId { get; set; }

    /// <summary>
    /// The parent resource, if the resource is not located at the top of hierarchy.
    /// </summary>
    public Resource? Parent { get; set; }

    /// <summary>
    /// A list of child resources directly derived from this resource.
    /// </summary>
    public IEnumerable<Resource> Children { get; set; } = new List<Resource>();

    /// <summary>
    /// A list of users and their roles which has access to this resource.
    /// </summary>
    public IEnumerable<UserResourceRoles> ResourceRoles { get; set; } = new List<UserResourceRoles>();
}
