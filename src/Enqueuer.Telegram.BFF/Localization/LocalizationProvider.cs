using Enqueuer.Telegram.Shared.Localization;

namespace Enqueuer.Telegram.BFF.Localization;

public class LocalizationProvider : ILocalizationProvider
{
    public ValueTask<string> GetMessageAsync(string key, MessageParameters messageParameters, CancellationToken cancellationToken)
    {
        var message = Resources.Messages.ResourceManager.GetString(key, messageParameters.Culture);
        if (message == null)
        {
            throw new ArgumentException($"Message key '{key}' is unknown.");
        }

        return ValueTask.FromResult(string.Format(message, messageParameters.Parameters));
    }
}
