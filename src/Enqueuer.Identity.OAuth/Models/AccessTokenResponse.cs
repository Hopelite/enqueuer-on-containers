namespace Enqueuer.Identity.OAuth.Models;

public class AccessTokenResponse
{
    internal AccessTokenResponse(string value, string type, TimeSpan expiresIn)
    {
        Value = value;
        Type = type;
        ExpiresIn = expiresIn;
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
}