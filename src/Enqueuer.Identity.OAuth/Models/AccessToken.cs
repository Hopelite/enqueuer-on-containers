using Enqueuer.Identity.OAuth.Extensions;

namespace Enqueuer.Identity.Authorization.OAuth;

/// <summary>
/// Represents the issued access token.
/// </summary>
public readonly struct AccessToken
{
    internal AccessToken(string value, string type, TimeSpan expiresIn)
    {
        Value = value.ThrowIfNullOrWhitespace(nameof(value));
        Type = type.ThrowIfNullOrWhitespace(nameof(type));
        ExpiresIn = ValidateLifetime(expiresIn);
    }

    /// <summary>
    /// The value of the access token.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// The type of the access token.
    /// </summary>
    public string Type { get; }

    /// <summary>
    /// The lifetime of the issued access token.
    /// </summary>
    public TimeSpan ExpiresIn { get; }

    private static TimeSpan ValidateLifetime(in TimeSpan expiresIn)
    {
        if (expiresIn.Ticks < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(expiresIn), "Access token lifetime can't be negative.");
        }

        return expiresIn;
    }
}
