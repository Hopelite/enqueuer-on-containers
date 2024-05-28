using Enqueuer.Telegram.BFF.Core.Models.Messages;
using Enqueuer.Telegram.BFF.Messages.Handlers;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Enqueuer.Telegram.BFF.Messages.Factories;

public class MessageHandlersFactory(IServiceProvider serviceProvider) : IMessageHandlersFactory
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public bool TryCreateMessageHandler(MessageContext messageContext, [NotNullWhen(returnValue: true)] out IMessageHandler? messageHandler)
    {
        messageHandler = null;
        if (messageContext.Command == null)
        {
            return false;
        }

        return TryCreateMessageHandler(messageContext.Command.Command, out messageHandler);
    }

    private bool TryCreateMessageHandler(string command, out IMessageHandler? messageHandler)
    {
        messageHandler = command switch
        {
            "/start" => throw new NotImplementedException(),
            "/help" => throw new NotImplementedException(),
            "/queue" or "/q" => _serviceProvider.GetRequiredService<QueueMessageHandler>(),
            "/createqueue" or "/creq" => _serviceProvider.GetRequiredService<CreateQueueMessageHandler>(),
            "/removequeue" or "/req" => _serviceProvider.GetRequiredService<RemoveQueueMessageHandler>(),
            "/enqueue" or "/enq" => _serviceProvider.GetRequiredService<EnqueueMessageHandler>(),
            "/dequeue" or "/deq" => _serviceProvider.GetRequiredService<DequeueMessageHandler>(),
            _ => null
        };

        return messageHandler != null;
    }
}
