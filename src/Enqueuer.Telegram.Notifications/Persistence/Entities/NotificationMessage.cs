namespace Enqueuer.Telegram.Notifications.Persistence.Entities;

/// <summary>
/// Contains localized notification message.
/// </summary>
public class NotificationMessage
{
    /// <summary>
    /// The language code of the message.
    /// </summary>
    public required string LanguageCode { get; set; }

    /// <summary>
    /// The unique identifier of the message in the language scope.
    /// </summary>
    public required string Key { get; set; }

    /// <summary>
    /// The notification message.
    /// </summary>
    public required string Value { get; set; }
}
