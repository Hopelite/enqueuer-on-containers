using System;
using Enqueuer.EventBus.Abstractions;

namespace Enqueuer.Telegram.Notifications.Contract.V1.Events
{
    public class ChatLanguageChangedEvent : IIntegrationEvent
    {
        public ChatLanguageChangedEvent(Guid id, DateTime timestamp, long chatId, string languageCode)
        {
            Id = id;
            Timestamp = timestamp;
            ChatId = chatId;
            LanguageCode = languageCode;
        }

        public string Name => nameof(ChatLanguageChangedEvent);

        public Guid Id { get; }

        public DateTime Timestamp { get; }

        public long ChatId { get; }

        public string LanguageCode { get; }
    }
}
