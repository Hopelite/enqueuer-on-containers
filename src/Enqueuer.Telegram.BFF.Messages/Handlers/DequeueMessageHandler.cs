using Enqueuer.Queueing.Contract.V1;
using Enqueuer.Queueing.Contract.V1.Commands;
using Enqueuer.Telegram.BFF.Core.Models.Extensions;
using Enqueuer.Telegram.BFF.Core.Models.Messages;
using Enqueuer.Telegram.BFF.Messages.Localization;
using Enqueuer.Telegram.Shared.Localization;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace Enqueuer.Telegram.BFF.Messages.Handlers;

public class DequeueMessageHandler(
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
            var errorMessage = _localizationProvider.GetMessage(MessageKeys.DequeueErrorMissingQueueName, new MessageParameters(messageContext.Chat.Culture));
            return _telegramClient.SendTextMessageAsync(
                chatId: messageContext.Chat.Id,
                text: errorMessage,
                parseMode: ParseMode.Html,
                cancellationToken: cancellationToken);
        }

        return _queueingClient.DequeueParticipant(messageContext.Chat.Id, queueContext.QueueName, new DequeueParticipantCommand(messageContext.Sender.Id), cancellationToken);
    }
}
