using System.Collections.Generic;
using Enqueuer.OAuth.Core.Tokens.Grants;

namespace Enqueuer.Identity.Contract.V1.OAuth.Models
{
    public interface IAccessTokenRequest
    {
        /// <summary>
        /// The authorization grant used in the request.
        /// </summary>
        public IAuthorizationGrant Grant { get; }

        /// <summary>
        /// Gets the query parameters based on the <see cref="Grant"/> values.
        /// </summary>
        public abstract IDictionary<string, string> GetQueryParameters();
    }
}
