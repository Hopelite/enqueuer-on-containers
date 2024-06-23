namespace Enqueuer.Telegram.Notifications.Persistence.Entities;

/// <summary>
/// The available language of chat notifications.
/// </summary>
public class Language
{
    public const string DefaultChatLanguage = "en";

    /// <summary>
    /// The unique identifier of the language.
    /// </summary>
    public string Code { get; set; } = DefaultChatLanguage;
}
