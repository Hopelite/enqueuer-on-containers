using Enqueuer.Telegram.BFF.Core.Models.Messages;

namespace Enqueuer.Telegram.BFF.Messages.Handlers;

/// <summary>
/// Defines the handler for the incoming messages.
/// </summary>
public interface IMessageHandler
{
    /// <summary>
    /// Handles the incoming message.
    /// </summary>
    Task HandleAsync(MessageContext messageContext, CancellationToken cancellationToken);
}