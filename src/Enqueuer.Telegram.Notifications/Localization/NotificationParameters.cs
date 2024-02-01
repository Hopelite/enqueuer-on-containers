using System.Globalization;

namespace Enqueuer.Telegram.Notifications.Localization;

/// <summary>
/// Contains notification parameters for <see cref="ILocalizationProvider"/>.
/// </summary>
internal class NotificationParameters
{
    /// <summary>
    /// Constant for empty parameters.
    /// </summary>
    public static readonly NotificationParameters None = new();

    /// <summary>
    /// Notification parameters to be inserted into the notification. Can be empty.
    /// </summary>
    public string[] Parameters { get; }

    /// <summary>
    /// Culture used to get the localized notification. By default set to the current thread culture.
    /// </summary>
    public CultureInfo Culture { get; }

    public NotificationParameters()
        : this(Array.Empty<string>())
    {
    }

    public NotificationParameters(CultureInfo? cultureInfo)
        : this(cultureInfo, Array.Empty<string>())
    {
    }

    public NotificationParameters(params string[] parameters)
        : this(Thread.CurrentThread.CurrentCulture, parameters)
    {
    }

    public NotificationParameters(CultureInfo? cultureInfo, params string[] parameters)
    {
        if (parameters.Any(p => string.IsNullOrWhiteSpace(p)))
        {
            throw new ArgumentException("One of the notification parameters is null, empty or a whitespace.");
        }

        Parameters = parameters;
        Culture = cultureInfo ?? Thread.CurrentThread.CurrentUICulture;
    }
}
