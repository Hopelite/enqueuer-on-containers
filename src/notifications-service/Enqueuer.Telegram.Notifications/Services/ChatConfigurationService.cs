using Enqueuer.EventBus.Abstractions;
using Enqueuer.Telegram.Notifications.Contract.V1.Events;
using Enqueuer.Telegram.Notifications.Contract.V1.Models;
using Enqueuer.Telegram.Notifications.Services.Factories;
using MongoDB.Driver;

namespace Enqueuer.Telegram.Notifications.Services;

public class ChatConfigurationService(IMongoClientFactory clientFactory, IEventBusClient busClient) : IChatConfigurationService
{
    private readonly IMongoCollection<ChatNotificationsConfiguration> _configurationCollection = clientFactory.GetCollection<ChatNotificationsConfiguration>("ChatConfiguration");
    private readonly IEventBusClient _busClient = busClient;

    public async Task ConfigureChatNotificationsAsync(ChatNotificationsConfiguration chatConfiguration, CancellationToken cancellationToken)
    {
        // TODO: check if language is supported
        var wasLanguageChanged = false;

        var existingChatConfiguration = await _configurationCollection.Find(c => c.ChatId == chatConfiguration.ChatId).FirstOrDefaultAsync(cancellationToken);
        if (existingChatConfiguration != null)
        {
            wasLanguageChanged = existingChatConfiguration.MessageLanguageCode != chatConfiguration.MessageLanguageCode;
            if (wasLanguageChanged)
            {
                var filter = Builders<ChatNotificationsConfiguration>.Filter
                    .Eq(c => c.ChatId, chatConfiguration.ChatId);

                var update = Builders<ChatNotificationsConfiguration>.Update
                    .Set(c => c.MessageLanguageCode, chatConfiguration.MessageLanguageCode);

                await _configurationCollection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
            }
        }
        else
        {
            wasLanguageChanged = true;
            await _configurationCollection.InsertOneAsync(chatConfiguration, cancellationToken: cancellationToken);
        }

        if (wasLanguageChanged)
        {
            var languageUpdatedEvent = new ChatLanguageChangedEvent(Guid.NewGuid(), DateTime.UtcNow, chatConfiguration.ChatId, chatConfiguration.MessageLanguageCode);
            await _busClient.PublishAsync(languageUpdatedEvent, cancellationToken);
        }
    }

    public async Task<ChatNotificationsConfiguration> GetChatConfigurationAsync(long chatId, string? languageCode = null, CancellationToken cancellationToken = default)
    {
        var chatConfiguration = await _configurationCollection.Find(c => c.ChatId == chatId).FirstOrDefaultAsync(cancellationToken);
        if (chatConfiguration == null)
        {
            // TODO: check if language is supported
            if (string.IsNullOrWhiteSpace(languageCode))
            {
                languageCode = ChatNotificationsConfiguration.DefaultChatLanguage;
            }

            chatConfiguration = new ChatNotificationsConfiguration() { ChatId = chatId, MessageLanguageCode = languageCode };
            await _configurationCollection.InsertOneAsync(chatConfiguration, cancellationToken: cancellationToken);
        }

        return chatConfiguration;
    }
}
