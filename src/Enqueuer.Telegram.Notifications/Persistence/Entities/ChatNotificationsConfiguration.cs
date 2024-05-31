namespace Enqueuer.Telegram.Notifications.Persistence.Entities;

/// <summary>
/// Contains configuration of the chat notifications.
/// </summary>
public class ChatNotificationsConfiguration
{
    /// <summary>
    /// The unique identifier of the chat to which this configuration is for.
    /// </summary>
    public long ChatId { get; set; }

    /// <summary>
    /// The code of the language used in the chat.
    /// </summary>
    public string NotificationsLanguageCode { get; set; } = Language.DefaultChatLanguage;

    /// <summary>
    /// The code of the language used in the chat.
    /// </summary>
    public Language NotificationsLanguage { get; set; } = null!;
}
