using Enqueuer.Telegram.Notifications.Persistence;
using Enqueuer.Telegram.Notifications.Persistence.Entities;

namespace Enqueuer.Telegram.Notifications.Services;

public class ChatConfigurationService : IChatConfigurationService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public ChatConfigurationService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public Task ConfigureChatNotificationsAsync(ChatNotificationsConfiguration chatConfiguration, CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var notificationsContext = scope.ServiceProvider.GetRequiredService<NotificationsContext>();

        if (notificationsContext.NotificationsConfigurations.Any(c => c.ChatId == chatConfiguration.ChatId))
        {
            notificationsContext.Update(chatConfiguration);
        }
        else
        {
            notificationsContext.Add(chatConfiguration);
        }

        return notificationsContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<ChatNotificationsConfiguration> GetChatNotificationsAsync(long chatId, CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var notificationsContext = scope.ServiceProvider.GetRequiredService<NotificationsContext>();

        var chatConfiguration = await notificationsContext.NotificationsConfigurations.FindAsync(new object[] { chatId }, cancellationToken);
        if (chatConfiguration == null)
        {
            chatConfiguration = new ChatNotificationsConfiguration() { ChatId = chatId };
            notificationsContext.Add(chatConfiguration);
            await notificationsContext.SaveChangesAsync(cancellationToken);
        }

        return chatConfiguration;
    }
}
