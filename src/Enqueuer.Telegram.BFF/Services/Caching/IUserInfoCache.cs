using Enqueuer.Telegram.BFF.Core.Models.Common;

namespace Enqueuer.Telegram.BFF.Services.Caching
{
    public interface IUserInfoCache
    {
        ValueTask<User?> GetUserInfoAsync(long userId, CancellationToken cancellationToken);

        ValueTask SetUserInfoAsync(User userInfo, CancellationToken cancellationToken);
    }
}
