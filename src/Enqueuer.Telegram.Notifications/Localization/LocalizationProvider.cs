using Enqueuer.Telegram.Shared.Localization;

namespace Enqueuer.Telegram.Notifications.Localization;

internal class LocalizationProvider : ILocalizationProvider
{
    public string GetMessage(string key, MessageParameters messageParameters)
    {
        var message = Resources.Messages.ResourceManager.GetString(key, messageParameters.Culture);
        if (message == null)
        {
            throw new ArgumentException($"Message key '{key}' is unknown.");
        }

        return string.Format(message, messageParameters.Parameters);
    }
}