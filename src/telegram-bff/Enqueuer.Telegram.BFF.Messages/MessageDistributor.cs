using Enqueuer.Telegram.BFF.Core.Models.Messages;
using Enqueuer.Telegram.BFF.Messages.Factories;

namespace Enqueuer.Telegram.BFF.Messages;

public class MessageDistributor(IMessageHandlersFactory messageHandlersFactory) : IMessageDistributor
{
    private readonly IMessageHandlersFactory _messageHandlersFactory = messageHandlersFactory;

    public Task DistributeAsync(MessageContext messageContext, CancellationToken cancellationToken)
    {
        if (_messageHandlersFactory.TryCreateMessageHandler(messageContext, out var handler))
        {
            return handler.HandleAsync(messageContext, cancellationToken);
        }

        return Task.CompletedTask;
    }
}