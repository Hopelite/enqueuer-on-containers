using Microsoft.AspNetCore.Mvc;

namespace Enqueuer.Identity.API.Parameters;

/// <summary>
/// The request to create new or update the existing user.
/// </summary>
public class CreateOrUpdateUserRequest
{
    /// <summary>
    /// The unique identifier of the user to create or update.
    /// </summary>
    [FromRoute(Name = "user_id")]
    public long UserId { get; set; }

    /// <summary>
    /// The first name of the user.
    /// </summary>
    public string FirstName { get; set; } = null!;

    /// <summary>
    /// The last name of the user.
    /// </summary>
    public string? LastName { get; set; }
}
