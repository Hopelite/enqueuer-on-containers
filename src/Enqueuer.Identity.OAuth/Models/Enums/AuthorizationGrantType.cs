namespace Enqueuer.Identity.OAuth.Models.Enums;

/// <summary>
/// The known grant type for token requests.
/// </summary>
public static class AuthorizationGrantType
{
    public const string AuthorizationCode = "authorization_code";

    public const string ClientCredentials = "client_credentials";

    public const string RefreshToken = "refresh_token";
}