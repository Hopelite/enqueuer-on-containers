using Enqueuer.OAuth.Core.Models;

namespace Enqueuer.Identity.Contract.V1.OAuth.Configuration
{
    public class ClientCredentialsAuthorizationOptions<TClient> : IClientAuthorizationOptions<TClient>
    {
        // TODO: add init when possible
        public string ClientId { get; set; } = null!;

        public string ClientSecret { get; set; } = null!;

        public Scope Scope => new Scope(RequiredScope);

        // Backing field for Scope to be set from configuration
        private string[] RequiredScope { get; set; } = null!;
    }
}
