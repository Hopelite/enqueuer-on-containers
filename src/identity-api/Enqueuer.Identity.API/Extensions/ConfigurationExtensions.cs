namespace Enqueuer.Identity.API.Extensions;

public static class ConfigurationExtensions
{
    public static Uri GetRequiredUri(this IConfiguration configuration, string key)
    {
        return configuration.GetValue<Uri>(key) ?? throw new ArgumentNullException();
    }
}
