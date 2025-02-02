using Enqueuer.Identity.Contract.V1.Models;

namespace Enqueuer.Identity.Contract.V1.Services
{
    public interface IUserInfoCache
    {
        ValueTask<UserInfo?> GetUserInfoAsync(long userId, CancellationToken cancellationToken);

        ValueTask SetUserInfoAsync(UserInfo userInfo, CancellationToken cancellationToken);
    }
}
