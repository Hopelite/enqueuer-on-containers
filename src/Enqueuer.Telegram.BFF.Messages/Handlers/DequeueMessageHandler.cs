using Enqueuer.Queueing.Contract.V1;
using Enqueuer.Queueing.Contract.V1.Commands;
using Enqueuer.Queueing.Contract.V1.Exceptions;
using Enqueuer.Telegram.BFF.Core.Models.Extensions;
using Enqueuer.Telegram.BFF.Core.Models.Messages;
using Enqueuer.Telegram.BFF.Messages.Localization;
using Enqueuer.Telegram.Shared.Localization;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace Enqueuer.Telegram.BFF.Messages.Handlers;

public class DequeueMessageHandler(
    IQueueingClient queueingClient,
    ITelegramBotClient telegramClient,
    ILocalizationProvider localizationProvider,
    ILogger<CreateQueueMessageHandler> logger) : MessageHandlerBase(telegramClient, localizationProvider)
{
    private readonly IQueueingClient _queueingClient = queueingClient;
    private readonly ILogger<CreateQueueMessageHandler> _logger = logger;

    public override async Task HandleAsync(MessageContext messageContext, CancellationToken cancellationToken)
    {
        var queueContext = messageContext.Command!.GetQueueName();

        if (string.IsNullOrEmpty(queueContext.QueueName))
        {
            var errorMessage = localizationProvider.GetMessage(MessageKeys.DequeueErrorMissingQueueName, new MessageParameters(messageContext.Chat.Culture));
            await telegramClient.SendTextMessageAsync(
                chatId: messageContext.Chat.Id,
                text: errorMessage,
                parseMode: ParseMode.Html,
                cancellationToken: cancellationToken);

            return;
        }

        try
        {
            await _queueingClient.DequeueParticipant(messageContext.Chat.Id, queueContext.QueueName, new DequeueParticipantCommand(messageContext.Sender.Id), cancellationToken);
        }
        catch (QueueDoesNotExistException)
        {
            var errorMessage = localizationProvider.GetMessage(MessageKeys.DequeueErrorQueueDoesNotExist, new MessageParameters(messageContext.Chat.Culture, queueContext.QueueName));
            await telegramClient.SendTextMessageAsync(
                chatId: messageContext.Chat.Id,
                text: errorMessage,
                parseMode: ParseMode.Html,
                cancellationToken: cancellationToken);
        }
        catch (ParticipantDoesNotExistException)
        {
            var errorMessage = localizationProvider.GetMessage(MessageKeys.DequeueErrorParticipantIsNotEnqueued, new MessageParameters(messageContext.Chat.Culture, queueContext.QueueName));
            await telegramClient.SendTextMessageAsync(
                chatId: messageContext.Chat.Id,
                text: errorMessage,
                parseMode: ParseMode.Html,
                cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occured when trying to dequeue participant with ID '{ParticipantId}' from the queue '{QueueName}' in the chat '{ChatId}'.",
                messageContext.Sender.Id, queueContext.QueueName, messageContext.Chat.Id);
            await NotifyUserAboutInternalErrorAsync(messageContext, cancellationToken);
            return;
        }
    }
}
