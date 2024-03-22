using Enqueuer.Telegram.Notifications.Persistence;
using Enqueuer.Telegram.Shared.Localization;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Enqueuer.Telegram.Notifications.Localization;

internal class LocalizationProvider : ILocalizationProvider
{
    private readonly ConcurrentDictionary<FormatMessageKey, string> _formatMessages = new();
    private readonly IServiceScopeFactory _scopeFactory;

    public LocalizationProvider(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public async ValueTask<string> GetMessageAsync(string key, MessageParameters notificationParameters, CancellationToken cancellationToken)
    {
        var formatKey = new FormatMessageKey(key, notificationParameters.Culture);
        if (!_formatMessages.TryGetValue(formatKey, out var format))
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var notificationsContext = scope.ServiceProvider.GetRequiredService<NotificationsContext>();
                format = (await notificationsContext.LocalizedMessages
                    .FindAsync(new object[] { notificationParameters.Culture.Name, key }, cancellationToken: cancellationToken))?.Value;

                if (format == null)
                {
                    throw new MessageDoesNotExistException($"Message '{key}' either does not exist or is not localized for the '{notificationParameters.Culture.Name}'.");
                }
            }

            _formatMessages.TryAdd(formatKey, format);
        }

        if (notificationParameters.Parameters.Length == 0)
        {
            return format;
        }

        return string.Format(format, notificationParameters.Parameters);
    }

    private readonly struct FormatMessageKey : IEquatable<FormatMessageKey>
    {
        private readonly string _key;
        private readonly string _cultureName;

        public FormatMessageKey(string key, CultureInfo cultureInfo)
            : this(key, cultureInfo.Name)
        {
        }

        public FormatMessageKey(string key, string cultureName)
        {
            _key = key;
            _cultureName = cultureName;
        }

        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (obj is FormatMessageKey messageKey)
            {
                return Equals(messageKey);
            }

            return false;
        }

        public bool Equals(FormatMessageKey messageKey)
        {
            return string.Equals(_key, messageKey._key, StringComparison.InvariantCulture)
                && string.Equals(_cultureName, messageKey._cultureName, StringComparison.CurrentCulture);
        }

        public override int GetHashCode()
        {
            const int firstPrimeNumber = 17;
            const int secondPrimeNumber = 23;

            unchecked
            {
                var hash = firstPrimeNumber;
                hash = hash * secondPrimeNumber + _key.GetHashCode();
                hash = hash * secondPrimeNumber + _cultureName.GetHashCode();
                return hash;
            }
        }
    }
}