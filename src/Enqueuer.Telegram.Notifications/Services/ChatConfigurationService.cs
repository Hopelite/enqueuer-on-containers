using Enqueuer.EventBus.Abstractions;
using Enqueuer.Telegram.Notifications.Contract.V1.Events;
using Enqueuer.Telegram.Notifications.Persistence;
using Enqueuer.Telegram.Notifications.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;

namespace Enqueuer.Telegram.Notifications.Services;

public class ChatConfigurationService(IServiceScopeFactory scopeFactory, IEventBusClient busClient) : IChatConfigurationService
{
    private readonly IServiceScopeFactory _scopeFactory = scopeFactory;
    private readonly IEventBusClient _busClient = busClient;
    private ImmutableHashSet<string>? _supportedLanguages;

    public async Task ConfigureChatNotificationsAsync(Contract.V1.Models.ChatNotificationsConfiguration chatConfiguration, CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var notificationsContext = scope.ServiceProvider.GetRequiredService<NotificationsContext>();

        if (await IsNotSupportedAsync(notificationsContext, chatConfiguration.MessageLanguageCode))
        {
            throw new Exception();
        }

        var wasLanguageChanged = false;

        var existingChatConfiguration = await notificationsContext.NotificationsConfigurations.FindAsync(new object[] { chatConfiguration.ChatId }, cancellationToken);
        if (existingChatConfiguration != null)
        {
            wasLanguageChanged = existingChatConfiguration.NotificationsLanguageCode != chatConfiguration.MessageLanguageCode;
            if (wasLanguageChanged)
            {
                existingChatConfiguration.NotificationsLanguageCode = chatConfiguration.MessageLanguageCode;
                notificationsContext.Update(existingChatConfiguration);
            }
        }
        else
        {
            wasLanguageChanged = true;
            notificationsContext.Add(ToEntity(chatConfiguration));
        }

        await notificationsContext.SaveChangesAsync(cancellationToken);
        if (wasLanguageChanged)
        {
            var languageUpdatedEvent = new ChatLanguageChangedEvent(Guid.NewGuid(), DateTime.UtcNow, chatConfiguration.ChatId, chatConfiguration.MessageLanguageCode);
            await _busClient.PublishAsync(languageUpdatedEvent, cancellationToken);
        }
    }

    public async Task<Contract.V1.Models.ChatNotificationsConfiguration> GetChatConfigurationAsync(long chatId, string? languageCode = null, CancellationToken cancellationToken = default)
    {
        using var scope = _scopeFactory.CreateScope();
        var notificationsContext = scope.ServiceProvider.GetRequiredService<NotificationsContext>();

        var chatConfiguration = await notificationsContext.NotificationsConfigurations.FindAsync(new object[] { chatId }, cancellationToken);
        if (chatConfiguration == null)
        {
            if (string.IsNullOrWhiteSpace(languageCode) || await IsNotSupportedAsync(notificationsContext, languageCode))
            {
                languageCode = Language.DefaultChatLanguage;
            }

            chatConfiguration = new ChatNotificationsConfiguration() { ChatId = chatId, NotificationsLanguageCode = languageCode };
            notificationsContext.Add(chatConfiguration);
            await notificationsContext.SaveChangesAsync(cancellationToken);
        }

        return ToViewModel(chatConfiguration);
    }

    private async ValueTask<bool> IsNotSupportedAsync(NotificationsContext notificationsContext, string languageCode)
    {
        if (_supportedLanguages == null)
        {
            var availableLanguages = await notificationsContext.AvailableLanguages.Select(l => l.Code).ToArrayAsync();
            _supportedLanguages = ImmutableHashSet.Create(availableLanguages);
        }

        return !_supportedLanguages.Contains(languageCode);
    }

    private static ChatNotificationsConfiguration ToEntity(Contract.V1.Models.ChatNotificationsConfiguration viewModel)
        => new()
        {
            ChatId = viewModel.ChatId,
            NotificationsLanguageCode = viewModel.MessageLanguageCode,
        };

    private static Contract.V1.Models.ChatNotificationsConfiguration ToViewModel(ChatNotificationsConfiguration entity)
        => new()
        {
            ChatId = entity.ChatId,
            MessageLanguageCode = entity.NotificationsLanguageCode
        };
}
