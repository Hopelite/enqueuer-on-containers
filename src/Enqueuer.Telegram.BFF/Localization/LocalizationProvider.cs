using Enqueuer.Telegram.Shared.Localization;

namespace Enqueuer.Telegram.BFF.Localization;

public class LocalizationProvider : ILocalizationProvider
{
    public ValueTask<string> GetMessageAsync(string key, MessageParameters messageParameters, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
