namespace Enqueuer.Identity.OAuth.Extensions;

internal static class StringExtensions
{
    public static string ThrowIfNullOrWhitespace(this string? value, string parameterName)
    {
        return string.IsNullOrWhiteSpace(value)
            ? throw new ArgumentNullException(parameterName, $"{parameterName} can't be null, empty or a whitespace.")
            : value;
    }
}
