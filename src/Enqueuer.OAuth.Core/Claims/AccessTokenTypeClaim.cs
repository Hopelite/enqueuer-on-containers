using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Enqueuer.OAuth.Core.Claims
{
    /// <summary>
    /// Represents the OAuth JWS media  type claim.
    /// </summary>
    public class AccessTokenTypeClaim : Claim
    {
        public const string MediaType = "at+jwt";

        private AccessTokenTypeClaim(string? issuer)
            : base(JwtRegisteredClaimNames.Typ, MediaType, ClaimValueTypes.String, issuer)
        {
        }

        /// <summary>
        /// Creates the <see cref="AccessTokenTypeClaim"/>.
        /// </summary>
        public static AccessTokenTypeClaim Create(string? issuer = default)
        {
            return new AccessTokenTypeClaim(issuer);
        }
    }
}
