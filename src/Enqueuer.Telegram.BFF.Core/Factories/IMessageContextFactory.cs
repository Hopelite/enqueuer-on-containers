using Enqueuer.Telegram.BFF.Core.Models.Messages;
using Telegram.Bot.Types;

namespace Enqueuer.Telegram.BFF.Core.Factories;

public interface IMessageContextFactory
{
    /// <summary>
    /// Creates <see cref="MessageContext"/> containing all required data related to the incoming <paramref name="message"/>. 
    /// </summary>
    /// <returns>Returns null, if was unable to create context for the <paramref name="message"/>.</returns>
    ValueTask<MessageContext?> CreateMessageContextAsync(Message message, CancellationToken cancellationToken0);
}
