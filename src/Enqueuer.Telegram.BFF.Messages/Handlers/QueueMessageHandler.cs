using System.Text;
using Enqueuer.Identity.Contract.V1;
using Enqueuer.Queueing.Contract.V1;
using Enqueuer.Queueing.Contract.V1.Exceptions;
using Enqueuer.Queueing.Contract.V1.Queries.ViewModels;
using Enqueuer.Telegram.BFF.Core.Models.Extensions;
using Enqueuer.Telegram.BFF.Core.Models.Messages;
using Enqueuer.Telegram.BFF.Messages.Localization;
using Enqueuer.Telegram.Shared.Localization;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace Enqueuer.Telegram.BFF.Messages.Handlers;

public class QueueMessageHandler(
    IQueueingClient queueingClient,
    ITelegramBotClient telegramClient,
    ILocalizationProvider localizationProvider,
    IIdentityClient identityClient) : IMessageHandler
{
    private readonly IQueueingClient _queueingClient = queueingClient;
    private readonly ITelegramBotClient _telegramClient = telegramClient;
    private readonly ILocalizationProvider _localizationProvider = localizationProvider;
    private readonly IIdentityClient _identityClient = identityClient;

    public Task HandleAsync(MessageContext messageContext, CancellationToken cancellationToken)
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
        var queues = await _queueingClient.GetGroupQueuesAsync(messageContext.Chat.Id, cancellationToken);

        if (queues.Count == 0)
        {
            var message = _localizationProvider.GetMessage(MessageKeys.QueueMessageGroupDoesNotHaveAnyQueue, new MessageParameters(messageContext.Chat.Culture));
            await _telegramClient.SendTextMessageAsync(
                    chatId: messageContext.Chat.Id,
                    text: message,
                    parseMode: ParseMode.Html,
                    cancellationToken: cancellationToken);

            return;
        }

        var messageWithQueues = ListGroupQueuesInMessage(messageContext, queues);
        await _telegramClient.SendTextMessageAsync(
            chatId: messageContext.Chat.Id,
            text: messageWithQueues,
            parseMode: ParseMode.Html,
            cancellationToken: cancellationToken);
    }

    private async Task ListQueueParticipantsAsync(MessageContext messageContext, QueueNameContext queueContext, CancellationToken cancellationToken)
    {
        try
        {
            var participants = await _queueingClient.GetQueueParticipantsAsync(messageContext.Chat.Id, queueContext.QueueName, cancellationToken);
            if (participants.Count == 0)
            {
                var message = _localizationProvider.GetMessage(MessageKeys.QueueMessageQueueIsEmpty, new MessageParameters(messageContext.Chat.Culture, queueContext.QueueName));
                await _telegramClient.SendTextMessageAsync(
                        chatId: messageContext.Chat.Id,
                        text: message,
                        parseMode: ParseMode.Html,
                        cancellationToken: cancellationToken);

                return;
            }

            var messageWithParticipants = await ListQueueParticipantsInMessage(messageContext, queueContext, participants, cancellationToken);
            await _telegramClient.SendTextMessageAsync(
                chatId: messageContext.Chat.Id,
                text: messageWithParticipants,
                parseMode: ParseMode.Html,
                cancellationToken: cancellationToken);
        }
        catch (QueueDoesNotExistException)
        {
            var errorMessage = localizationProvider.GetMessage(MessageKeys.QueueErrorQueueDoesNotExist, new MessageParameters(messageContext.Chat.Culture, queueContext.QueueName));
            await telegramClient.SendTextMessageAsync(
                chatId: messageContext.Chat.Id,
                text: errorMessage,
                parseMode: ParseMode.Html,
                cancellationToken: cancellationToken);
        }
    }

    private string ListGroupQueuesInMessage(MessageContext messageContext, IReadOnlyCollection<Queue> queues)
    {
        var messageHeader = _localizationProvider.GetMessage(MessageKeys.QueueMessageListGroupQueuesHeader, new MessageParameters(messageContext.Chat.Culture));
        var messageBuilder = new StringBuilder(messageHeader);

        messageBuilder.AppendLine();
        foreach (var queue in queues)
        {
            messageBuilder.AppendLine($"- {queue.Name}");
        }

        var messageFooter = _localizationProvider.GetMessage(MessageKeys.QueueMessageListGroupQueuesFooter, new MessageParameters(messageContext.Chat.Culture));
        messageBuilder.AppendLine(messageFooter);

        return messageBuilder.ToString();
    }

    private async ValueTask<string> ListQueueParticipantsInMessage(MessageContext messageContext, QueueNameContext queueContext, IReadOnlyCollection<Participant> participants, CancellationToken cancellationToken)
    {
        var messageHeader = _localizationProvider.GetMessage(MessageKeys.QueueMessageListQueueParticipantsHeader, new MessageParameters(messageContext.Chat.Culture, queueContext.QueueName));
        var messageBuilder = new StringBuilder(messageHeader);
        messageBuilder.AppendLine();

        foreach (var participant in participants)
        {
            var participantInfo = await _identityClient.GetUserInfoAsync(participant.Id, cancellationToken);
            messageBuilder.AppendLine($"{participant.Position}) {participantInfo.FirstName}"); // TODO: move to localization provider
        }

        var messageFooter = _localizationProvider.GetMessage(MessageKeys.QueueMessageListQueueParticipantsFooter, new MessageParameters(messageContext.Chat.Culture, queueContext.QueueName));
        messageBuilder.AppendLine(messageFooter);

        return messageBuilder.ToString();
    }
}
