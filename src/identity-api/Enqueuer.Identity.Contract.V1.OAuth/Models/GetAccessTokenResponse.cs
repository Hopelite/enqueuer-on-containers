using System.Text.Json.Serialization;

namespace Enqueuer.Identity.Contract.V1.OAuth.Models
{
    internal class GetAccessTokenResponse
    {
        [JsonConstructor]
        internal GetAccessTokenResponse(string accessToken, string tokenType, int expiresIn)
        {
            AccessToken = accessToken;
            TokenType = tokenType;
            ExpiresIn = expiresIn;
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
}
