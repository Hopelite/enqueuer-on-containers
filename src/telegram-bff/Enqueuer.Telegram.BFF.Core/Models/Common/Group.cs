namespace Enqueuer.Telegram.BFF.Core.Models.Common;

/// <summary>
/// Represents a Telegram group.
/// </summary>
public class Group : Chat
{
    /// <summary>
    /// Optional. The title of this group. Null, if the chat is private.
    /// </summary>
    public string? Title { get; init; }

    /// <summary>
    /// The type of this chat.
    /// </summary>
    public required ChatType Type { get; init; }
}
