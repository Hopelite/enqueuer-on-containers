using Enqueuer.Queueing.Contract.V1;
using Enqueuer.Queueing.Contract.V1.Queries.ViewModels;
using Enqueuer.Telegram.BFF.Core.Models.Extensions;
using Enqueuer.Telegram.BFF.Core.Models.Messages;
using Enqueuer.Telegram.BFF.Messages.Localization;
using Enqueuer.Telegram.Shared.Localization;
using Microsoft.Extensions.Logging;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace Enqueuer.Telegram.BFF.Messages.Handlers;

public class QueueMessageHandler(
    IQueueingClient queueingClient,
    ITelegramBotClient telegramClient,
    ILocalizationProvider localizationProvider,
    ILogger<QueueMessageHandler> logger) : MessageHandlerBase(telegramClient, localizationProvider)
{
    private readonly IQueueingClient _queueingClient = queueingClient;
    private readonly ILogger<QueueMessageHandler> _logger = logger;

    public override Task HandleAsync(MessageContext messageContext, CancellationToken cancellationToken)
    {
        var queueContext = messageContext.Command!.GetQueueName();

        if (string.IsNullOrEmpty(queueContext.QueueName))
        {
            return ListGroupQueuesAsync(messageContext, cancellationToken);
        }

        return ListQueueParticipantsAsync(messageContext, queueContext, cancellationToken);
    }

    private async Task ListGroupQueuesAsync(MessageContext messageContext, CancellationToken cancellationToken)
    {
        try
        {
            var queues = await _queueingClient.GetGroupQueuesAsync(messageContext.Chat.Id, cancellationToken);

            if (queues.Count == 0)
            {
                var message = await localizationProvider.GetMessageAsync(MessageKeys.QueueMessageGroupDoesNotHaveAnyQueue, new MessageParameters(messageContext.Chat.Culture), cancellationToken);
                await telegramClient.SendTextMessageAsync(
                        chatId: messageContext.Chat.Id,
                        text: message,
                        parseMode: ParseMode.Html,
                        cancellationToken: cancellationToken);

                return;
            }

            var messageWithQueues = await ListGroupQueuesInMessage(messageContext, queues, cancellationToken);
            await telegramClient.SendTextMessageAsync(
                chatId: messageContext.Chat.Id,
                text: messageWithQueues,
                parseMode: ParseMode.Html,
                cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occured during chat '{ChatId}' queues listing.", messageContext.Chat.Id);
            await NotifyUserAboutInternalErrorAsync(messageContext, cancellationToken);
        }
    }

    private async Task ListQueueParticipantsAsync(MessageContext messageContext, QueueNameContext queueContext, CancellationToken cancellationToken)
    {
        try
        {
            var participants = await _queueingClient.GetQueueParticipantsAsync(messageContext.Chat.Id, queueContext.QueueName, cancellationToken);
            if (participants.Count == 0)
            {
                var message = await localizationProvider.GetMessageAsync(MessageKeys.QueueMessageQueueIsEmpty, new MessageParameters(messageContext.Chat.Culture, queueContext.QueueName), cancellationToken);
                await telegramClient.SendTextMessageAsync(
                        chatId: messageContext.Chat.Id,
                        text: message,
                        parseMode: ParseMode.Html,
                        cancellationToken: cancellationToken);

                return;
            }

            throw new NotImplementedException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occured during queue '{QueueName}' participants listing in the chat '{ChatId}'.", queueContext.QueueName, messageContext.Chat.Id);
            await NotifyUserAboutInternalErrorAsync(messageContext, cancellationToken);
        }
    }

    private async ValueTask<string> ListGroupQueuesInMessage(MessageContext messageContext, IReadOnlyCollection<Queue> queues, CancellationToken cancellationToken)
    {
        var messageHeader = await localizationProvider.GetMessageAsync(MessageKeys.QueueMessageListGroupQueuesHeader, new MessageParameters(messageContext.Chat.Culture), cancellationToken);
        var messageBuilder = new StringBuilder(messageHeader);

        messageBuilder.AppendLine();
        foreach (var queue in queues)
        {
            messageBuilder.AppendLine(queue.Name);
        }

        var messageFooter = await localizationProvider.GetMessageAsync(MessageKeys.QueueMessageListGroupQueuesFooter, new MessageParameters(messageContext.Chat.Culture), cancellationToken);
        messageBuilder.AppendLine(messageFooter);

        return messageBuilder.ToString();
    }
}
