using Enqueuer.Queueing.Contract.V1;
using Enqueuer.Telegram.BFF.Core.Models.Messages;
using Enqueuer.Telegram.Shared.Localization;
using Microsoft.Extensions.Logging;
using Telegram.Bot;

namespace Enqueuer.Telegram.BFF.Messages.Handlers;

public class EnqueueMessageHandler(
    IQueueingClient queueingClient,
    ITelegramBotClient telegramClient,
    ILocalizationProvider localizationProvider,
    ILogger<CreateQueueMessageHandler> logger) : IMessageHandler
{
    public async Task HandleAsync(MessageContext messageContext, CancellationToken cancellationToken)
    {


        throw new NotImplementedException();
    }
}
