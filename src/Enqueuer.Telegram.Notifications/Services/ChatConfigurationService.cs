using Enqueuer.Telegram.Notifications.Persistence;
using Enqueuer.Telegram.Notifications.Persistence.Entities;

namespace Enqueuer.Telegram.Notifications.Services;

public class ChatConfigurationService(IServiceScopeFactory scopeFactory) : IChatConfigurationService
{
    private readonly IServiceScopeFactory _scopeFactory = scopeFactory;

    public Task ConfigureChatNotificationsAsync(Contract.V1.Models.ChatNotificationsConfiguration chatConfiguration, CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var notificationsContext = scope.ServiceProvider.GetRequiredService<NotificationsContext>();

        var chatConfigurationEntity = ToEntity(chatConfiguration);
        if (notificationsContext.NotificationsConfigurations.Any(c => c.ChatId == chatConfigurationEntity.ChatId))
        {
            notificationsContext.Update(chatConfigurationEntity);
        }
        else
        {
            notificationsContext.Add(chatConfigurationEntity);
        }

        return notificationsContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Contract.V1.Models.ChatNotificationsConfiguration> GetChatConfigurationAsync(long chatId, CancellationToken cancellationToken)
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

        return ToViewModel(chatConfiguration);
    }

    private static ChatNotificationsConfiguration ToEntity(Contract.V1.Models.ChatNotificationsConfiguration viewModel)
        => new()
        {
            ChatId = viewModel.ChatId,
            NotificationsLanguageCode = viewModel.NotificationsLanguageCode,
        };

    private static Contract.V1.Models.ChatNotificationsConfiguration ToViewModel(ChatNotificationsConfiguration entity)
        => new()
        {
            ChatId = entity.ChatId,
            NotificationsLanguageCode = entity.NotificationsLanguageCode
        };
}
