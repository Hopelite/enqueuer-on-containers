namespace Enqueuer.OAuth.Core.Enums
{
    /// <summary>
    /// Contains the list of OAuth grant types.
    /// </summary>
    public static class AuthorizationGrantType
    {
        public const string GrantTypeParameter = "grant_type";

        /// <summary>
        /// Contains the list of client credentials grant parameters.
        /// </summary>
        public static class ClientCredentials
        {
            public const string Type = "client_credentials";

            public const string ClientIdParameter = "client_id";

            public const string ClientSecretParameter = "client_secret";
        }

        /// <summary>
        /// Contains the list of authorization code grant parameters.
        /// </summary>
        public static class AuthorizationCode
        {
            public const string Type = "authorization_code";
        }

        /// <summary>
        /// Contains the list of refresh token grant parameters.
        /// </summary>
        public static class RefreshToken
        {
            public const string Type = "refresh_token";
        }
    }
}
