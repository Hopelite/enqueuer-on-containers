namespace Enqueuer.Telegram.BFF.Core.Models.Common;

/// <summary>
/// Represents a Telegram chat with a user.
/// </summary>
public class User : Chat
{
    /// <summary>
    /// The first name of the user.
    /// </summary>
    public required string FirstName { get; init; }

    /// <summary>
    /// Optional. The last name of the user.
    /// </summary>
    public string? LastName { get; init; }

    /// <summary>
    /// Gets the full name of the user.
    /// </summary>
    public string FullName => string.IsNullOrWhiteSpace(LastName) ? FirstName : $"{FirstName} {LastName}";
}
