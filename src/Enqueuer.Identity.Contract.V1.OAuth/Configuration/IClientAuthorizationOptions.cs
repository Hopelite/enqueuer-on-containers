using Enqueuer.OAuth.Core.Models;

namespace Enqueuer.Identity.Contract.V1.OAuth.Configuration
{
    /// <summary>
    /// Contains options for a typed HTTP <typeparamref name="TClient"/>.
    /// </summary>
    public interface IClientAuthorizationOptions<TClient>
    {
        /// <summary>
        /// The scope required to be included in access tokens for the <typeparamref name="TClient"/>.
        /// </summary>
        public Scope RequiredScope { get; }
    }
}
