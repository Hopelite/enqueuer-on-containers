using System.Collections.Generic;
using Enqueuer.OAuth.Core.Claims;
using Enqueuer.OAuth.Core.Enums;
using Enqueuer.OAuth.Core.Tokens.Grants;

namespace Enqueuer.Identity.Contract.V1.OAuth.Models
{
    public class ClientCredentialsTokenRequest : IAccessTokenRequest
    {
        private readonly ClientCredentialsGrant _grant;

        public ClientCredentialsTokenRequest(ClientCredentialsGrant grant)
        {
            _grant = grant;
        }

        public IAuthorizationGrant Grant => _grant;

        public IDictionary<string, string> GetQueryParameters()
        {
            var parameters = new Dictionary<string, string>()
            {
                { AuthorizationGrantType.GrantTypeParameter,                        AuthorizationGrantType.ClientCredentials.Type },
                { AuthorizationGrantType.ClientCredentials.ClientIdParameter,       _grant.ClientId },
                { AuthorizationGrantType.ClientCredentials.ClientSecretParameter,   _grant.ClientSecret }
            };

            var scope = _grant.Scope;
            if (scope.HasValue)
            {
                parameters.Add(ScopeClaim.ClaimType, scope.Value!);
            }

            return parameters;
        }
    }
}
