using Enqueuer.EventBus.Abstractions;
using Enqueuer.Telegram.BFF.Core.Configuration;
using Enqueuer.Telegram.BFF.Core.Models.Configuration;
using Enqueuer.Telegram.Notifications.Contract.V1.Events;

namespace Enqueuer.Telegram.BFF.Services.Configuration;

public class GroupConfigurationRefresher(IChatConfigurationCache configurationCache) : IntegrationEventHandlerBase<ChatLanguageChangedEvent>
{
    private readonly IChatConfigurationCache _configurationCache = configurationCache;

    public override async Task HandleAsync(ChatLanguageChangedEvent @event, CancellationToken cancellationToken)
    {
        var newConfiguration = new ChatMessagingConfiguration
        {
            ChatId = @event.ChatId,
            LanguageCode = @event.LanguageCode,
        };

        await _configurationCache.SetGroupConfigurationAsync(newConfiguration, cancellationToken);
    }
}
