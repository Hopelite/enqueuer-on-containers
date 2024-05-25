namespace Enqueuer.Identity.OAuth.Models;

public class AccessTokenResponse
{
    internal AccessTokenResponse(AccessToken accessToken)
    {
        Value = accessToken.Value;
        Type = accessToken.Type;
        ExpiresIn = accessToken.ExpiresIn;
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
