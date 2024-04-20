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

public class CreateQueueMessageHandler(
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
            var errorMessage = await localizationProvider.GetMessageAsync(MessageKeys.CreateQueueErrorMissingQueueName, MessageParameters.None, cancellationToken);
            await telegramClient.SendTextMessageAsync(
                chatId: messageContext.Chat.Id,
                text: errorMessage,
                parseMode: ParseMode.Html,
                cancellationToken: cancellationToken);

            return;
        }

        try
        {
            await _queueingClient.CreateQueueAsync(messageContext.Chat.Id, queueContext.QueueName, cancellationToken);
        }
        catch (ResourceAlreadyExistsException)
        {
            var errorMessage = await localizationProvider.GetMessageAsync(MessageKeys.CreateQueueErrorQueueAlreadyExists, new MessageParameters(queueContext.QueueName), cancellationToken);
            await telegramClient.SendTextMessageAsync(
                chatId: messageContext.Chat.Id,
                text: errorMessage,
                parseMode: ParseMode.Html,
                cancellationToken: cancellationToken);

            return;
        }
        catch (InvalidQueueNameException)
        {
            var errorMessage = await localizationProvider.GetMessageAsync(MessageKeys.CreateQueueErrorInvalidQueueName, MessageParameters.None, cancellationToken);
            await telegramClient.SendTextMessageAsync(
                chatId: messageContext.Chat.Id,
                text: errorMessage,
                parseMode: ParseMode.Html,
                cancellationToken: cancellationToken);
            
            return;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occured during queue '{QueueName}' creation in the chat '{ChatId}'.", queueContext.QueueName, messageContext.Chat.Id);
            await NotifyUserAboutInternalErrorAsync(messageContext, cancellationToken);
            return;
        }

        if (queueContext.Position.HasValue)
        {
           await EnqueueUserOnSpecifiedPosition(messageContext, queueContext, cancellationToken);
        }
    }

    private async Task EnqueueUserOnSpecifiedPosition(MessageContext messageContext, QueueNameContext queueContext, CancellationToken cancellationToken)
    {        
        // TODO: extract validation to separate class
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
            _logger.LogError(ex, "An error occured when trying to enqueue participant with ID '{ParticipantId}' to the new queue '{QueueName}' to position '{Position}' in the chat '{ChatId}'.",
                messageContext.Sender.Id, queueContext.QueueName, queueContext.Position.Value, messageContext.Chat.Id);
            await NotifyUserAboutInternalErrorAsync(messageContext, cancellationToken);
        }
    }
}
