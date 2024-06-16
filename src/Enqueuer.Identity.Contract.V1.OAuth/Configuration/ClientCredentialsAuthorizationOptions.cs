using Enqueuer.OAuth.Core.Models;

namespace Enqueuer.Identity.Contract.V1.OAuth.Configuration
{
    public class ClientCredentialsAuthorizationOptions<TClient> : IClientAuthorizationOptions<TClient>
    {
        private readonly Scope _scope;

        public string ClientId { get; }

        public string ClientSecret { get; }

        public Scope RequiredScope => _scope;
    }
}
