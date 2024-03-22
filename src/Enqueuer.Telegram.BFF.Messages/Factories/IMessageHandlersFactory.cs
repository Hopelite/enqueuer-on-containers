using Enqueuer.Telegram.BFF.Core.Models.Messages;
using Enqueuer.Telegram.BFF.Messages.Handlers;
using System.Diagnostics.CodeAnalysis;

namespace Enqueuer.Telegram.BFF.Messages.Factories;

/// <summary>
/// Creates message handlers to handle incoming messages.
/// </summary>
public interface IMessageHandlersFactory
{
    /// <summary>
    /// Tries to create an appropriate message handler for the <paramref name="messageContext"/>.
    /// </summary>
    bool TryCreateMessageHandler(MessageContext messageContext, [NotNullWhen(returnValue: true)] out IMessageHandler? messageHandler);
}

