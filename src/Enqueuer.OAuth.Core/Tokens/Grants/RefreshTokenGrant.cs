using Enqueuer.OAuth.Core.Enums;
using Enqueuer.OAuth.Core.Models;

namespace Enqueuer.OAuth.Core.Tokens.Grants
{
    /// <summary>
    /// The authorization grant used when client requests to refresh previously issued access token.
    /// </summary>
    public class RefreshTokenGrant : IAuthorizationGrant
    {
        public RefreshTokenGrant(string refreshToken, Scope scope)
        {
            RefreshToken = refreshToken;
            Scope = scope;
        }

        public string Type => AuthorizationGrantType.RefreshToken.Type;

        /// <summary>
        /// The refresh token issued to the client.
        /// </summary>
        public string RefreshToken { get; }

        /// <summary>
        /// The scope of the access request.
        /// </summary>
        /// <remarks>
        /// The requested scope MUST NOT include any scope not originally granted by the resource owner,
        /// and if omitted is treated as equal to the scope originally granted by the resource owner.
        /// </remarks>
        public Scope Scope { get; }
    }
}
