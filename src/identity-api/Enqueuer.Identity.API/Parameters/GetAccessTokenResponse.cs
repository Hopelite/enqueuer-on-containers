namespace Enqueuer.Identity.API.Parameters;

public class GetAccessTokenResponse
{
    internal GetAccessTokenResponse(string accessToken, string tokenType, TimeSpan expiresIn)
    {
        AccessToken = accessToken;
        TokenType = tokenType;
        ExpiresIn = (int)expiresIn.TotalSeconds; // TODO: consider to reject seconds longer than int.Max
    }

    /// <summary>
    /// The access token issued by the authorization server.
    /// </summary>
    public string AccessToken { get; }

    /// <summary>
    /// The type of the token issued.
    /// </summary>
    public string TokenType { get; }

    /// <summary>
    /// The lifetime in seconds of the access token.
    /// </summary>
    public int ExpiresIn { get; }
}
