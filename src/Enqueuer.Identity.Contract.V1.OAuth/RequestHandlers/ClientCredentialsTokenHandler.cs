using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Enqueuer.Identity.Contract.V1.OAuth.Configuration;
using Enqueuer.Identity.Contract.V1.OAuth.Models;
using Enqueuer.OAuth.Core.Tokens;
using Enqueuer.OAuth.Core.Tokens.Grants;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Enqueuer.Identity.Contract.V1.OAuth.RequestHandlers
{
    public class ClientCredentialsTokenHandler<TClient> : DelegatingHandler
    {
        private readonly IOAuthClient _oauthClient;
        private readonly ILogger<ClientCredentialsTokenHandler<TClient>> _logger;
        private readonly ClientCredentialsAuthorizationOptions<TClient> _authorizationOptions;
        private AccessToken? _accessToken;

        public ClientCredentialsTokenHandler(
            IOAuthClient oauthClient,
            IOptions<ClientCredentialsAuthorizationOptions<TClient>> authorizationOptions,
            // TODO: add token cache here, since handler is scoped/transient
            ILogger<ClientCredentialsTokenHandler<TClient>> logger)
        {
            _oauthClient = oauthClient;
            _logger = logger;
            _authorizationOptions = authorizationOptions.Value;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (_accessToken == null || _accessToken.HasExpired)
            {
                var authorizationGrant = new ClientCredentialsGrant(_authorizationOptions.ClientId, _authorizationOptions.ClientSecret, _authorizationOptions.Scope);

                _logger.LogDebug("Request new access token for the client '{ClientId}'.", authorizationGrant.ClientId);
                _accessToken = await _oauthClient.GetAccessTokenAsync(new ClientCredentialsTokenRequest(authorizationGrant), cancellationToken);
                _logger.LogDebug("Successfully requested new access token for the client '{ClientId}'.", authorizationGrant.ClientId);
            }

            request.Headers.Authorization = new AuthenticationHeaderValue(_accessToken.Type, _accessToken.Value);
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
