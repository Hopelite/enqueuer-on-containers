using Enqueuer.OAuth.Core.Models;
using System.Collections.Generic;
using System.Security.Claims;

namespace Enqueuer.OAuth.Core.Claims
{
    /// <summary>
    /// Represents the OAuth scope claim.
    /// </summary>
    public class ScopeClaim : Claim
    {
        public const string ClaimType = "scope";

        public ScopeClaim(Scope scope, string? issuer)
            : base(ClaimType, scope.Value, ClaimValueTypes.String, issuer)
        {
            Scope = scope;
        }

        /// <summary>
        /// The scope set in this claim.
        /// </summary>
        public Scope Scope { get; }

        /// <summary>
        /// Checks whether this scope contains the specified <paramref name="value"/>.
        /// </summary>
        public bool Contains(string value)
        {
            return Scope.Contains(value);
        }

        /// <summary>
        /// Creates a <see cref="ScopeClaim"/> containing the specified <paramref name="values"/>.
        /// </summary>
        public static ScopeClaim Create(IReadOnlyCollection<string> values, string? issuer = default)
        {
            var scope = new Scope(values);
            return new ScopeClaim(scope, issuer);
        }

        /// <summary>
        /// Creates a single <see cref="ScopeClaim"/> containing the specified values of the <paramref name="value"/> string.
        /// </summary>
        public static ScopeClaim Create(string value, string? issuer = default)
        {
            var scope = new Scope(value);
            return new ScopeClaim(scope, issuer);
        }
    }
}
