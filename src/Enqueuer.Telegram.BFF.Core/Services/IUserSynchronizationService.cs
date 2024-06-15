using Enqueuer.Telegram.BFF.Core.Models.Common;
using Enqueuer.Telegram.BFF.Core.Models.Messages;

namespace Enqueuer.Telegram.BFF.Core.Services;

public interface IUserSynchronizationService
{
    /// <summary>
    /// Synchronizes the actual user information (i.e. first name) with internal systems.
    /// </summary>
    ValueTask SynchronizeUserInfoAsync(MessageContext messageContext, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the info of a user with the specified <paramref name="userId"/>.
    /// </summary>
    /// <remarks>Retrieved user info may be outdated and unsynchronized due to eventual consistency.</remarks>
    ValueTask<User> GetUserInfoAsync(long userId, CancellationToken cancellationToken);
}
