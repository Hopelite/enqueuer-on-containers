namespace Enqueuer.Telegram.BFF.Core.Models.Configuration;

/// <summary>
/// Contains the configuration for messages sent to chat.
/// </summary>
public class ChatMessagingConfiguration
{
    public required long ChatId { get; init; }

    public required string LanguageCode { get; init; }
}
