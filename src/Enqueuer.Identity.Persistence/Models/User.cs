namespace Enqueuer.Identity.Persistence.Models;

/// <summary>
/// Represents Telegram user. 
/// </summary>
public class User
{
    /// <summary>
    /// The unique identifier of the user.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// The first name of the user.
    /// </summary>
    public string FirstName { get; set; }

    /// <summary>
    /// Options. The last name of the user.
    /// </summary>
    public string? LastName { get; set; }
}
