namespace Enqueuer.Telegram.Notifications.Contract.V1.Models
{
    /// <summary>
    /// View model containing configuration of the chat notifications.
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
        public string MessageLanguageCode { get; set; } = null!;
    }
}
