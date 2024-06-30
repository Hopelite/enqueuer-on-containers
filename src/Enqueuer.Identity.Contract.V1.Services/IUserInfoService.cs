using Enqueuer.Identity.Contract.V1.Models;

namespace Enqueuer.Identity.Contract.V1.Services;

public interface IUserInfoService
{
    /// <summary>
    /// Gets the info of a user with the specified <paramref name="userId"/>.
    /// </summary>
    /// <remarks>Retrieved user info may be outdated and unsynchronized due to eventual consistency.</remarks>
    ValueTask<UserInfo> GetUserInfoAsync(long userId, CancellationToken cancellationToken);
}
