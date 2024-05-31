using Enqueuer.EventBus.Abstractions;
using Enqueuer.Telegram.BFF.Core.Configuration;
using Enqueuer.Telegram.BFF.Core.Models.Configuration;
using Enqueuer.Telegram.Notifications.Contract.V1.Events;

namespace Enqueuer.Telegram.BFF.Services.Caching;

public class GroupConfigurationRefresher : IntegrationEventHandlerBase<ChatLanguageChangedEvent>
{
    private readonly IChatConfigurationCache _configurationCache;

    public GroupConfigurationRefresher(IChatConfigurationCache configurationCache)
    {
        _configurationCache = configurationCache;
    }

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
