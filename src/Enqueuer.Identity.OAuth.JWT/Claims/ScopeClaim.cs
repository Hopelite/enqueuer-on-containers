using System.Security.Claims;
using Enqueuer.Identity.OAuth.Models;

namespace Enqueuer.Identity.OAuth.JWT.Claims
{
    /// <summary>
    /// Represents the OAuth scope claim.
    /// </summary>
    public class ScopeClaim : Claim
    {
        private const string ScopeClaimType = "scope";

        public ScopeClaim(Scope scope)
            : base(ScopeClaimType, scope.Value, ClaimValueTypes.String)
        {
        }
    }
}
