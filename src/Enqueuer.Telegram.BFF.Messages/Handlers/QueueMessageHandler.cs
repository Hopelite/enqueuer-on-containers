using Enqueuer.Queueing.Contract.V1;
using Enqueuer.Telegram.BFF.Core.Models.Extensions;
using Enqueuer.Telegram.BFF.Core.Models.Messages;
using Enqueuer.Telegram.BFF.Messages.Localization;
using Enqueuer.Telegram.Shared.Localization;
using Microsoft.Extensions.Logging;
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

        if (queueContext == null)
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
                var message = await localizationProvider.GetMessageAsync(MessageKeys.QueueMessageGroupDoesNotHaveAnyQueue, MessageParameters.None, cancellationToken);
                await telegramClient.SendTextMessageAsync(
                    chatId: messageContext.Chat.Id,
                        text: message,
                        parseMode: ParseMode.Html,
                        cancellationToken: cancellationToken);

                return;
            }

            // List queues
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
                var message = await localizationProvider.GetMessageAsync(MessageKeys.QueueMessageQueueIsEmpty, new MessageParameters(queueContext.QueueName), cancellationToken);
                await telegramClient.SendTextMessageAsync(
                    chatId: messageContext.Chat.Id,
                        text: message,
                        parseMode: ParseMode.Html,
                        cancellationToken: cancellationToken);

                return;
            }

            // List participants
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occured during queue '{QueueName}' participants listing in the chat '{ChatId}'.", queueContext.QueueName, messageContext.Chat.Id);
            await NotifyUserAboutInternalErrorAsync(messageContext, cancellationToken);
        }
    }
}
