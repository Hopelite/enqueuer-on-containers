using Enqueuer.Identity.Contract.V1.Models;

namespace Enqueuer.Identity.Contract.V1.Services;

public class UserInfoService(IIdentityClient identityClient, IUserInfoCache userInfoCache) : IUserInfoService
{
    private readonly IIdentityClient _identityClient = identityClient;
    private readonly IUserInfoCache _userInfoCache = userInfoCache;

    public async ValueTask<UserInfo> GetUserInfoAsync(long userId, CancellationToken cancellationToken)
    {
        var cachedUserInfo = await _userInfoCache.GetUserInfoAsync(userId, cancellationToken);
        if (cachedUserInfo != null)
        {
            return cachedUserInfo;
        }

        var actualUserInfo = await _identityClient.GetUserInfoAsync(userId, cancellationToken);
        if (actualUserInfo.Metadata.MaxAge.Seconds > 0)
        {
            await _userInfoCache.SetUserInfoAsync(actualUserInfo, cancellationToken);
        }

        return actualUserInfo;
    }
}
