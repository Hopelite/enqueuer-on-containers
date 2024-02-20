using Enqueuer.Telegram.BFF.Core.Models.Messages;

namespace Enqueuer.Telegram.BFF.Messages.Handlers;

public class EnqueueMessageHandler : IMessageHandler
{
    public Task HandleAsync(MessageContext messageContext, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
