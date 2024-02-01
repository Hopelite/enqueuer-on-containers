using System.Globalization;

namespace Enqueuer.Telegram.Notifications.Localization;

internal interface ILocalizationProvider
{
    /// <summary>
    /// Gets a formatted message by <paramref name="key"/> with the specified <paramref name="messageParameters"/>.
    /// </summary>
    string GetMessage(string key, NotificationParameters messageParameters);
}
