using Enqueuer.Queueing.Contract.V1;
using Enqueuer.Queueing.Contract.V1.Commands;
using Enqueuer.Telegram.BFF.Core.Models.Extensions;
using Enqueuer.Telegram.BFF.Core.Models.Messages;
using Enqueuer.Telegram.BFF.Messages.Localization;
using Enqueuer.Telegram.Shared.Localization;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace Enqueuer.Telegram.BFF.Messages.Handlers;

public class RemoveQueueMessageHandler(
    IQueueingClient queueingClient,
    ITelegramBotClient telegramClient,
    ILocalizationProvider localizationProvider,
    ILogger<CreateQueueMessageHandler> logger) : IMessageHandler
{
    private readonly IQueueingClient _queueingClient = queueingClient;
    private readonly ITelegramBotClient _telegramClient = telegramClient;
    private readonly ILocalizationProvider _localizationProvider = localizationProvider;
    private readonly ILogger<CreateQueueMessageHandler> _logger = logger;

    public async Task HandleAsync(MessageContext messageContext, CancellationToken cancellationToken)
    {
        var queueContext = messageContext.Command!.GetQueueName();

        if (string.IsNullOrEmpty(queueContext.QueueName))
        {
            var errorMessage = await _localizationProvider.GetMessageAsync(MessageKeys.DeleteQueueErrorMissingQueueName, MessageParameters.None, cancellationToken);
            await _telegramClient.SendTextMessageAsync(
                chatId: messageContext.Chat.Id,
                text: errorMessage,
                parseMode: ParseMode.Html,
                cancellationToken: cancellationToken);

            return;
        }

        try
        {
            await _queueingClient.DeleteGroupQueue(new DeleteGroupQueueCommand(messageContext.Chat.Id, queueContext.QueueName), cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occured during queue '{QueueName}' deletion in the chat '{ChatId}'.", queueContext.QueueName, messageContext.Chat.Id);

            var errorMessage = await _localizationProvider.GetMessageAsync(MessageKeys.GeneralErrorInternal, MessageParameters.None, cancellationToken);
            await _telegramClient.SendTextMessageAsync(
                chatId: messageContext.Chat.Id,
                text: errorMessage,
                parseMode: ParseMode.Html,
                cancellationToken: cancellationToken);

            return;
        }
    }
}
