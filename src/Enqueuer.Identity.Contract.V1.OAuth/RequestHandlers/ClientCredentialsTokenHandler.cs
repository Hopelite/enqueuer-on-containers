using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Enqueuer.Identity.Contract.V1.OAuth.Configuration;
using Enqueuer.Identity.Contract.V1.OAuth.Models;
using Enqueuer.OAuth.Core.Tokens;
using Enqueuer.OAuth.Core.Tokens.Grants;
using Microsoft.Extensions.Logging;

namespace Enqueuer.Identity.Contract.V1.OAuth.RequestHandlers
{
    public class ClientCredentialsTokenHandler<TClient> : DelegatingHandler
    {
        private readonly IOAuthClient _oauthClient;
        private readonly ClientCredentialsAuthorizationOptions<TClient> _authorizationOptions;
        private AccessToken? _accessToken;

        public ClientCredentialsTokenHandler(IOAuthClient oauthClient, ClientCredentialsAuthorizationOptions<TClient> authorizationOptions, ILogger<ClientCredentialsTokenHandler<TClient>> logger)
        {
            _oauthClient = oauthClient;
            _authorizationOptions = authorizationOptions;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (_accessToken == null || _accessToken.HasExpired)
            {
                var authorizationGrant = new ClientCredentialsGrant(_authorizationOptions.ClientId, _authorizationOptions.ClientSecret, _authorizationOptions.RequiredScope);
                _accessToken = await _oauthClient.GetAccessTokenAsync(new ClientCredentialsTokenRequest(authorizationGrant), cancellationToken);
            }

            request.Headers.Authorization = new AuthenticationHeaderValue(_accessToken.Type, _accessToken.Value);
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
