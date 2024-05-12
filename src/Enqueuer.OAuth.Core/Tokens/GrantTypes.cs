namespace Enqueuer.OAuth.Core.Tokens
{
    /// <summary>
    /// Contains the list of OAuth grant types.
    /// </summary>
    public static class GrantTypes
    {
        public const string GrantTypeParameter = "grant_type";

        /// <summary>
        /// Contains the list of client credentials grant parameters.
        /// </summary>
        public static class ClientCredentialsGrant
        {
            public const string Type = "client_credentials";

            public const string ClientIdParameter = "client_id";

            public const string ClientSecretParameter = "client_secret";
        }
    }
}
