namespace Enqueuer.Telegram.Notifications.Models;

/// <summary>
/// Contains chat language.
/// </summary>
public class ChatLanguage
{
    /// <summary>
    /// The language code of the chat.
    /// </summary>
    public required string LanguageCode { get; init; }
}
