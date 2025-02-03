using System.Globalization;

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

    /// <summary>
    /// The language code of the chat messages language.
    /// </summary>
    public /*required*/ string LanguageCode { get; init; }

    public CultureInfo Culture => new CultureInfo(LanguageCode);
}
