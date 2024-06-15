namespace Enqueuer.Identity.OAuth.Tokens;

public class AccessToken
{
    public AccessToken(string value, string type, TimeSpan expiresIn)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value, nameof(value));
        ArgumentException.ThrowIfNullOrWhiteSpace(type, nameof(type));

        Value = value;
        Type = type;
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
