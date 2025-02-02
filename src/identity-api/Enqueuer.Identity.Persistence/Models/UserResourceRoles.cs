namespace Enqueuer.Identity.Persistence.Models;

/// <summary>
/// Represents the user's access to the specific resource by their role.
/// </summary>
public class UserResourceRoles
{
    /// <summary>
    /// The unique identifier of the user whose resource access is represented.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// The user whose resource access is represented.
    /// </summary>
    public User User { get; set; }

    /// <summary>
    /// The unique identifier of the resource within the database the user has access to.
    /// </summary>
    public int ResourceId { get; set; }

    /// <summary>
    /// The resource within the database the user has access to.
    /// </summary>
    public Resource Resource { get; set; }

    /// <summary>
    /// The unique identifier of the role user has regarding this resource.
    /// </summary>
    public int RoleId { get; set; }

    /// <summary>
    /// The role user has regarding this resource.
    /// </summary>
    public Role Role { get; set; }
}
