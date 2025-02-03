using Enqueuer.Telegram.BFF.Core.Models.Messages;

namespace Enqueuer.Telegram.BFF.Messages;

/// <summary>
/// Distributes messages to message handlers.
/// </summary>
public interface IMessageDistributor
{
    /// <summary>
    /// Distributes the <paramref name="messageContext"/> to an appropriate <see cref="IMessageHandler"/>, if exists.
    /// </summary>
    Task DistributeAsync(MessageContext messageContext, CancellationToken cancellationToken);
}