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

public class EnqueueMessageHandler(
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
            var errorMessage = await localizationProvider.GetMessageAsync(MessageKeys.EnqueueErrorMissingQueueName, MessageParameters.None, cancellationToken);
            await telegramClient.SendTextMessageAsync(
                chatId: messageContext.Chat.Id,
                text: errorMessage,
                parseMode: ParseMode.Html,
                cancellationToken: cancellationToken);

            return;
        }

        if (queueContext.Position.HasValue)
        {
            await EnqueueUserOnPositionAsync(messageContext, queueContext, cancellationToken);
            return;
        }

        await EnqueueUserOnFirstAvailablePositionAsync(messageContext, queueContext, cancellationToken);
    }

    private async Task EnqueueUserOnPositionAsync(MessageContext messageContext, QueueNameContext queueContext, CancellationToken cancellationToken)
    {
        // TODO: extract validation to separate other class
        if (queueContext.Position!.Value <= 0)
        {
            var errorMessage = await localizationProvider.GetMessageAsync(MessageKeys.CreateQueueErrorPositionMustBePositive, MessageParameters.None, cancellationToken);
            await telegramClient.SendTextMessageAsync(
                chatId: messageContext.Chat.Id,
                text: errorMessage,
                parseMode: ParseMode.Html,
                cancellationToken: cancellationToken);

            return;
        }

        try
        {
            await _queueingClient.EnqueueParticipantTo(messageContext.Chat.Id, queueContext.QueueName, (uint)queueContext.Position.Value, new EnqueueParticipantToCommand(messageContext.Sender.Id), cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occured when trying to enqueue participant with ID '{ParticipantId}' to the queue '{QueueName}' to position '{Position}' in the chat '{ChatId}'.",
                messageContext.Sender.Id, queueContext.QueueName, queueContext.Position.Value, messageContext.Chat.Id);
            await NotifyUserAboutInternalErrorAsync(messageContext, cancellationToken);
        }
    }

    private async Task EnqueueUserOnFirstAvailablePositionAsync(MessageContext messageContext, QueueNameContext queueContext, CancellationToken cancellationToken)
    {
        // TODO: consider structure logs like in Seq with tags like ChatId, User, Interaction, etc. to enhance the telemetry and logging
        try
        {
            await _queueingClient.EnqueueParticipant(messageContext.Chat.Id, queueContext.QueueName, new EnqueueParticipantCommand(messageContext.Sender.Id), cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occured when trying to enqueue participant with ID '{ParticipantId}' to the queue '{QueueName}' to the first available position in the chat '{ChatId}'.",
                messageContext.Sender.Id, queueContext.QueueName, messageContext.Chat.Id);
            await NotifyUserAboutInternalErrorAsync(messageContext, cancellationToken);
        }
    }
}
