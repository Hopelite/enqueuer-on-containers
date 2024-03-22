namespace Enqueuer.Telegram.BFF.Core.Models.Common;

/// <summary>
/// Represents a Telegram chat of any type.
/// </summary>
public abstract class Chat
{
    /// <summary>
    /// Unique identifier of this chat.
    /// </summary>
    public required long Id { get; init; }
}
