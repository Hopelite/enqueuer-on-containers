namespace Enqueuer.Telegram.Shared.Localization;

public interface ILocalizationProvider
{
    /// <summary>
    /// Gets a formatted message by <paramref name="key"/> with the specified <paramref name="messageParameters"/>.
    /// </summary>
    ValueTask<string> GetMessageAsync(string key, MessageParameters messageParameters, CancellationToken cancellationToken);
}
