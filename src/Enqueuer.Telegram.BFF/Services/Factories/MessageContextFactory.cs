using Enqueuer.Telegram.BFF.Core.Configuration;
using Enqueuer.Telegram.BFF.Core.Factories;
using Enqueuer.Telegram.BFF.Core.Models.Common;
using Enqueuer.Telegram.BFF.Core.Models.Configuration;
using Enqueuer.Telegram.BFF.Core.Models.Extensions;
using Enqueuer.Telegram.BFF.Core.Models.Messages;
using Enqueuer.Telegram.Notifications.Contract.V1;
using Telegram.Bot.Types;

namespace Enqueuer.Telegram.BFF.Services.Factories;

public class MessageContextFactory : IMessageContextFactory
{
    private readonly IChatConfigurationCache _configurationCache;
    private readonly IChatConfigurationClient _configurationClient;

    public MessageContextFactory(IChatConfigurationCache configurationCache, IChatConfigurationClient configurationClient)
    {
        _configurationCache = configurationCache;
        _configurationClient = configurationClient;
    }

    public ValueTask<MessageContext?> CreateMessageContextAsync(Message message, CancellationToken cancellationToken)
    {
        if (message?.Text == null || message.From == null)
        {
            return ValueTask.FromResult<MessageContext?>(null);
        }

        if (!message.Text.TryGetCommand(out var command))
        {
            // TODO: disable when plain text messaging is completed (ex. Queue name for Callback button)
            return ValueTask.FromResult<MessageContext?>(null);
        }

        return CreateMessageContextAsyncCore(message, command, cancellationToken);
    }

    private async ValueTask<MessageContext?> CreateMessageContextAsyncCore(Message message, CommandContext command, CancellationToken cancellationToken)
    {
        var groupConfiguration = await GetConfigurationAsync(message.Chat.Id, message.From!.LanguageCode, cancellationToken);
        var userConfiguration = groupConfiguration;
        if (message.Chat.Id != message.From.Id)
        {
            userConfiguration = await GetConfigurationAsync(message.From.Id, message.From.LanguageCode, cancellationToken);
        }

        return new MessageContext
        {
            MessageId = message.MessageId,
            Command = command,
            Sender = new Core.Models.Common.User
            {
                Id = message.From.Id,
                LanguageCode = userConfiguration.LanguageCode,
                FirstName = message.From.FirstName,
                LastName = message.From.LastName,
            },
            Chat = new Group
            {
                Id = message.Chat.Id,
                LanguageCode = groupConfiguration.LanguageCode,
                Title = message.Chat.Title!,
                Type = (ChatType)message.Chat.Type,
            }
        };
    }

    private async ValueTask<ChatMessagingConfiguration> GetConfigurationAsync(long chatId, string? languageCode, CancellationToken cancellationToken)
    {
        var cachedGroupConfiguration = await _configurationCache.GetGroupConfigurationAsync(chatId, cancellationToken);
        if (cachedGroupConfiguration == null)
        {
            var groupConfiguration = await _configurationClient.GetNewOrExistingConfigurationAsync(chatId, languageCode, cancellationToken);
            cachedGroupConfiguration = new ChatMessagingConfiguration
            {
                ChatId = chatId,
                LanguageCode = groupConfiguration.MessageLanguageCode
            };

            await _configurationCache.SetGroupConfigurationAsync(cachedGroupConfiguration, cancellationToken);
        }

        return cachedGroupConfiguration;
    }
}
