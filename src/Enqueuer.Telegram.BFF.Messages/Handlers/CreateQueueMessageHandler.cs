using Enqueuer.Queueing.Contract.V1;
using Enqueuer.Queueing.Contract.V1.Commands;
using Enqueuer.Telegram.BFF.Core.Models.Extensions;
using Enqueuer.Telegram.BFF.Core.Models.Messages;
using Enqueuer.Telegram.BFF.Messages.Localization;
using Enqueuer.Telegram.Shared.Localization;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace Enqueuer.Telegram.BFF.Messages.Handlers;

public class CreateQueueMessageHandler(
    IQueueingClient queueingClient,
    ITelegramBotClient telegramClient,
    ILocalizationProvider localizationProvider) : IMessageHandler
{
    private readonly IQueueingClient _queueingClient = queueingClient;
    private readonly ITelegramBotClient _telegramClient = telegramClient;
    private readonly ILocalizationProvider _localizationProvider = localizationProvider;

    // TODO: consider to use Command handlers instead with the Command models (to remove the parsing and validation duplication)
    public async Task HandleAsync(MessageContext messageContext, CancellationToken cancellationToken)
    {
        var queueContext = messageContext.Command!.GetQueueName();

        if (string.IsNullOrEmpty(queueContext.QueueName))
        {
            var errorMessage = _localizationProvider.GetMessage(MessageKeys.CreateQueueErrorMissingQueueName, new MessageParameters(messageContext.Chat.Culture));
            await _telegramClient.SendTextMessageAsync(
                chatId: messageContext.Chat.Id,
                text: errorMessage,
                parseMode: ParseMode.Html,
                cancellationToken: cancellationToken);

            return;
        }

        // TODO: move to validator class
        if (queueContext.QueueName.Length > 64)
        {
            var errorMessage = _localizationProvider.GetMessage(MessageKeys.CreateQueueErrorInvalidQueueName, new MessageParameters(messageContext.Chat.Culture));
            await _telegramClient.SendTextMessageAsync(
                chatId: messageContext.Chat.Id,
                text: errorMessage,
                parseMode: ParseMode.Html,
                cancellationToken: cancellationToken);

            return;
        }

        await _queueingClient.CreateQueueAsync(messageContext.Chat.Id, queueContext.QueueName, new CreateQueueCommand(messageContext.Sender.Id), cancellationToken);
        if (queueContext.Position.HasValue)
        {
           await EnqueueUserOnSpecifiedPosition(messageContext, queueContext, cancellationToken);
        }
    }

    private Task EnqueueUserOnSpecifiedPosition(MessageContext messageContext, QueueNameContext queueContext, CancellationToken cancellationToken)
    {        
        // TODO: extract validation to separate class
        if (queueContext.Position!.Value <= 0)
        {
            var errorMessage = _localizationProvider.GetMessage(MessageKeys.CreateQueueErrorPositionMustBePositive, new MessageParameters(messageContext.Chat.Culture));
            return _telegramClient.SendTextMessageAsync(
                chatId: messageContext.Chat.Id,
                text: errorMessage,
                parseMode: ParseMode.Html,
                cancellationToken: cancellationToken);
        }

        return _queueingClient.EnqueueParticipantAt(messageContext.Chat.Id, queueContext.QueueName, (uint)queueContext.Position.Value, new EnqueueParticipantAtCommand(messageContext.Sender.Id), cancellationToken);
    }
}
