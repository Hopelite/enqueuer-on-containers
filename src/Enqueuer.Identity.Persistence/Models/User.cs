using System.Collections.Generic;

namespace Enqueuer.Identity.Persistence.Models;

/// <summary>
/// Represents the Telegram user. 
/// </summary>
public class User
{
    /// <summary>
    /// The unique identifier of the user within the database.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The unique identifier of the Telegram user.
    /// </summary>
    public long UserId { get; set; }

    // TODO: consider to extract FirstName and LastName to separate

    /// <summary>
    /// The first name of the user.
    /// </summary>
    public string FirstName { get; set; }

    /// <summary>
    /// Optional. The last name of the user.
    /// </summary>
    public string? LastName { get; set; }

    /// <summary>
    /// A list of resources this user has access to.
    /// </summary>
    public IEnumerable<UserResourceRoles> ResourceRoles { get; set; } = new List<UserResourceRoles>();
}
