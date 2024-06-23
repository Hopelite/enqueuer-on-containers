using Enqueuer.Identity.Contract.V1.Models;

namespace Enqueuer.Telegram.Notifications.Extensions;

internal static class UserInfoExtensions
{
    /// <summary>
    /// Gets user's full name out of <paramref name="userInfo"/>.
    /// </summary>
    public static string GetFullName(this UserInfo userInfo)
        => string.IsNullOrWhiteSpace(userInfo.LastName) ? userInfo.FirstName : $"{userInfo.FirstName} {userInfo.LastName}";
}
