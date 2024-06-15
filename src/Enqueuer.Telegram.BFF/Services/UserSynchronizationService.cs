using Enqueuer.Identity.Contract.V1;
using Enqueuer.Identity.Contract.V1.Models;
using Enqueuer.Telegram.BFF.Core.Models.Common;
using Enqueuer.Telegram.BFF.Core.Models.Messages;
using Enqueuer.Telegram.BFF.Core.Services;
using Enqueuer.Telegram.BFF.Services.Caching;

namespace Enqueuer.Telegram.BFF.Services;

public class UserSynchronizationService : IUserSynchronizationService
{
    private readonly IIdentityClient _identityClient;
    private readonly IUserInfoCache _userInfoCache;
    private readonly ILogger<UserSynchronizationService> _logger;

    public UserSynchronizationService(IIdentityClient identityClient, IUserInfoCache userInfoCache, ILogger<UserSynchronizationService> logger)
    {
        _identityClient = identityClient;
        _userInfoCache = userInfoCache;
        _logger = logger;
    }

    public async ValueTask SynchronizeUserInfoAsync(MessageContext messageContext, CancellationToken cancellationToken)
    {
        try
        {
            var userInfo = await _userInfoCache.GetUserInfoAsync(messageContext.Sender.Id, cancellationToken);
            if (userInfo == null || IsOutdated(userInfo, messageContext.Sender))
            {
                await _userInfoCache.SetUserInfoAsync(messageContext.Sender, cancellationToken);

                await _identityClient.CreateOrUpdateUserAsync(new CreateOrUpdateUserRequest(
                    messageContext.Sender.Id,
                    messageContext.Sender.FirstName,
                    messageContext.Sender.LastName), cancellationToken);
            }
        }
        catch (Exception ex)
        {
            // Synchronization errors should not break the system
            _logger.LogError(ex, "An error occured during user '{UserId}' synchronization.", messageContext.Sender.Id);
        }
    }

    public async ValueTask<User> GetUserInfoAsync(long userId, CancellationToken cancellationToken)
    {
        var cachedUserInfo = await _userInfoCache.GetUserInfoAsync(userId, cancellationToken);
        if (cachedUserInfo != null)
        {
            return cachedUserInfo;
        }

        var actualUserInfo = await _identityClient.GetUserInfoAsync(userId, cancellationToken);
        var user = new User
        {
            Id = actualUserInfo.UserId,
            FirstName = actualUserInfo.FirstName,
            LastName = actualUserInfo.LastName,
            //LanguageCode =
        };

        if (actualUserInfo.Metadata.MaxAge.Seconds > 0)
        {
            await _userInfoCache.SetUserInfoAsync(user, cancellationToken);
        }

        return user;
    }

    private static bool IsOutdated(User cachedUser, User actualUser)
    {
        return !cachedUser.FirstName.Equals(actualUser.FirstName)
            && string.Equals(cachedUser.LastName, actualUser.LastName);
    }
}
