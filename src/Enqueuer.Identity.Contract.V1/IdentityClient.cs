using Enqueuer.Identity.Contract.V1.Caching;
using Enqueuer.Identity.Contract.V1.Models;
using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Enqueuer.Identity.Contract.V1
{
    public class IdentityClient : IIdentityClient
    {
        private readonly HttpClient _httpClient;
        private readonly IAccessTokenCache _tokenCache;
        private readonly IdentityClientOptions _options;

        public IdentityClient(HttpClient httpClient, IAccessTokenCache tokenCache, IOptions<IdentityClientOptions> options)
        {
            _httpClient = httpClient;
            _tokenCache = tokenCache;
            _options = options.Value;
        }

        public async Task<AccessToken> GetAccessTokenAsync(CancellationToken cancellationToken)
        {
            var uri = new Uri($"oauth2/token" +
                $"?client_id=TelegramBFF&client_secret=11dc4555-edfa-4c44-b87f-53f9907f5ded" +
                $"&grant_type=client_credentials" +
                $"&scope=queue group user", UriKind.Relative);

            var response = await _httpClient.PostAsync(uri, null, cancellationToken);

            var json = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception();
            }

            var tokenResponse = JsonSerializer.Deserialize<GetAccessTokenResponse>(json, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower });
            if (tokenResponse == null)
            {
                throw new Exception();
            }

            return new AccessToken(tokenResponse.AccessToken, tokenResponse.TokenType, TimeSpan.FromSeconds(tokenResponse.ExpiresIn));
        }

        public async Task<bool> CheckAccessAsync(CheckAccessRequest request, CancellationToken cancellationToken)
        {
            await RefreshAccessTokenIfNeededAsync(cancellationToken);
            var result = await _httpClient.GetAsync($"api/authorization/{request.ResourceId}?user_id={request.UserId}&scope={request.Scope}", cancellationToken);

            if (result == null)
            {
                throw new Exception();
            }

            if (result.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }

            if (result.StatusCode == HttpStatusCode.NotFound)
            {
                return false;
            }

            throw new Exception();
        }

        private ValueTask RefreshAccessTokenIfNeededAsync(CancellationToken cancellationToken)
        {
            if (!_options.CacheToken)
            {
                // Request token here
                return RefreshTokenAsync(cancellationToken);
            }

            var token = _tokenCache.GetAccessToken();
            if (token != null && !token.HasExpired)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token.Type, token.Value);
                return default;
            }

            return RefreshTokenAsync(cancellationToken);
        }

        private async ValueTask RefreshTokenAsync(CancellationToken cancellationToken)
        {
            var token = await GetAccessTokenAsync(cancellationToken);
            if (_options.CacheToken)
            {
                _tokenCache.SetAccessToken(token);
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token.Type, token.Value);
        }
    }
}
