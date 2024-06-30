using Enqueuer.Queueing.Contract.V1;
using Enqueuer.Queueing.Contract.V1.Commands;
using Enqueuer.Telegram.BFF.Core.Models.Extensions;
using Enqueuer.Telegram.BFF.Core.Models.Messages;
using Enqueuer.Telegram.BFF.Messages.Localization;
using Enqueuer.Telegram.Shared.Localization;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace Enqueuer.Telegram.BFF.Messages.Handlers;

public class EnqueueMessageHandler(
    IQueueingClient queueingClient,
    ITelegramBotClient telegramClient,
    ILocalizationProvider localizationProvider) : IMessageHandler
{
    private readonly IQueueingClient _queueingClient = queueingClient;
    private readonly ITelegramBotClient _telegramClient = telegramClient;
    private readonly ILocalizationProvider _localizationProvider = localizationProvider;

    public Task HandleAsync(MessageContext messageContext, CancellationToken cancellationToken)
    {
        var queueContext = messageContext.Command!.GetQueueName();

        if (string.IsNullOrEmpty(queueContext.QueueName))
        {
            var errorMessage = _localizationProvider.GetMessage(MessageKeys.EnqueueErrorMissingQueueName, new MessageParameters(messageContext.Chat.Culture));
            return _telegramClient.SendTextMessageAsync(
                chatId: messageContext.Chat.Id,
                text: errorMessage,
                parseMode: ParseMode.Html,
                cancellationToken: cancellationToken);
        }

        if (queueContext.Position.HasValue)
        {
            return EnqueueUserOnPositionAsync(messageContext, queueContext, cancellationToken);
        }

        return _queueingClient.EnqueueParticipant(messageContext.Chat.Id, queueContext.QueueName, new EnqueueParticipantCommand(messageContext.Sender.Id), cancellationToken);
    }

    private Task EnqueueUserOnPositionAsync(MessageContext messageContext, QueueNameContext queueContext, CancellationToken cancellationToken)
    {
        // TODO: extract validation to separate other class
        if (queueContext.Position!.Value <= 0)
        {
            var errorMessage = _localizationProvider.GetMessage(MessageKeys.CreateQueueErrorPositionMustBePositive, new MessageParameters(messageContext.Chat.Culture));

            // TODO: consider refactor these calls
            return _telegramClient.SendTextMessageAsync(
                chatId: messageContext.Chat.Id,
                text: errorMessage,
                parseMode: ParseMode.Html,
                cancellationToken: cancellationToken);
        }

        return _queueingClient.EnqueueParticipantAt(messageContext.Chat.Id, queueContext.QueueName, (uint)queueContext.Position.Value, new EnqueueParticipantAtCommand(messageContext.Sender.Id), cancellationToken);
    }
}
