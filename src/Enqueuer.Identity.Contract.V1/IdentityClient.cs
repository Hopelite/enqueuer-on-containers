using Enqueuer.Identity.Contract.V1.Caching;
using Enqueuer.Identity.Contract.V1.Exceptions;
using Enqueuer.Identity.Contract.V1.Models;
using Enqueuer.OAuth.Core.Claims;
using Enqueuer.OAuth.Core.Tokens;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
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

        // TODO: reduce token scope to required by each client only
        private static readonly string[] RequiredScopes = new string[] { "queue", "group", "user", "access" };

        public IdentityClient(HttpClient httpClient, IAccessTokenCache tokenCache, IOptions<IdentityClientOptions> options)
        {
            _httpClient = httpClient;
            _tokenCache = tokenCache;
            _options = options.Value;
        }

        public async Task<AccessToken> GetAccessTokenAsync(IReadOnlyCollection<string> scopes, CancellationToken cancellationToken)
        {
            var response = await _httpClient.PostAsync(GetAccessTokenUrl(scopes), content: null, cancellationToken);

            var responseBody = await response.Content.ReadAsStringAsync();
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new InvalidCredentialsException($"The provided credentials are invalid. Reason: {responseBody}");
            }

            if (!response.IsSuccessStatusCode)
            {
                throw new IdentityClientException($"The request to get access token was not successful. Reason: {response.StatusCode}, {responseBody}");
            }

            var tokenResponse = JsonSerializer.Deserialize<GetAccessTokenResponse>(responseBody, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower });
            if (tokenResponse == null)
            {
                throw new IdentityClientException("Unable to deserialize the GetAccessToken response.");
            }

            return new AccessToken(tokenResponse.AccessToken, tokenResponse.TokenType, TimeSpan.FromSeconds(tokenResponse.ExpiresIn));
        }

        public async Task<bool> CheckAccessAsync(CheckAccessRequest request, CancellationToken cancellationToken)
        {
            await RefreshAccessTokenIfNeededAsync(cancellationToken);

            var uri = GetUrlWithQuery($"api/authorization/access/{request.ResourceId}", request.GetQueryParameters());
            var response = await _httpClient.GetAsync(uri, cancellationToken);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return false;
            }

            var responseBody = await response.Content.ReadAsStringAsync();
            throw new IdentityClientException($"The request to check access was not successful. Reason: {response.StatusCode}, {responseBody}");
        }

        public async Task<UserInfo> GetUserInfoAsync(long userId, CancellationToken cancellationToken)
        {
            await RefreshAccessTokenIfNeededAsync(cancellationToken);

            var uri = new Uri($"api/authorization/users/{userId}", UriKind.Relative);

            var response = await _httpClient.GetAsync(uri, cancellationToken);
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new InvalidCredentialsException($"Authorization error. Reason: {GetUnauthorizedErrorDescription(response.Headers)}");
            }

            var responseBody = await response.Content.ReadAsStringAsync();
            var userInfo = JsonSerializer.Deserialize<UserInfo>(responseBody, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower });
            if (userInfo == null)
            {
                // TODO: extract to single method
                throw new IdentityClientException("Unable to deserialize the GetUserInfo response.");
            }

            return userInfo;
        }

        public async Task CreateOrUpdateUserAsync(CreateOrUpdateUserRequest request, CancellationToken cancellationToken)
        {
            await RefreshAccessTokenIfNeededAsync(cancellationToken);

            var uri = new Uri($"api/authorization/users/{request.UserId}", UriKind.Relative);

            var response = await _httpClient.PutAsync(uri, request.GetBody(), cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                return;
            }

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new InvalidCredentialsException($"Authorization error. Reason: {GetUnauthorizedErrorDescription(response.Headers)}");
            }

            var responseBody = await response.Content.ReadAsStringAsync();
            throw new IdentityClientException($"The request to create or update user was not successful. Reason: {response.StatusCode}, {responseBody}");
        }

        public async Task GrantAccessAsync(GrantAccessRequest request, CancellationToken cancellationToken)
        {
            await RefreshAccessTokenIfNeededAsync(cancellationToken);

            var uri = GetUrlWithQuery($"api/authorization/access/{request.ResourceId}", request.GetQueryParameters());
            var response = await _httpClient.PutAsync(uri, content: null, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                return;
            }

            var responseBody = await response.Content.ReadAsStringAsync();
            throw new IdentityClientException($"The request to grant access was not successful. Reason: {response.StatusCode}, {responseBody}");
        }

        public async Task RevokeAccessAsync(RevokeAccessRequest request, CancellationToken cancellationToken)
        {
            await RefreshAccessTokenIfNeededAsync(cancellationToken);

            var uri = GetUrlWithQuery($"api/authorization/access/{request.ResourceId}", request.GetQueryParameters());
            var response = await _httpClient.DeleteAsync(uri, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                return;
            }

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new InvalidCredentialsException($"Authorization error. Reason: {GetUnauthorizedErrorDescription(response.Headers)}");
            }

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                // If access does not exist - it's OK, do not throw exception
                return;
            }

            var responseBody = await response.Content.ReadAsStringAsync();
            throw new IdentityClientException($"The request to revoke access was not successful. Reason: {response.StatusCode}, {responseBody}");
        }

        private ValueTask RefreshAccessTokenIfNeededAsync(CancellationToken cancellationToken)
        {
            if (!_options.CacheToken)
            {
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
            var token = await GetAccessTokenAsync(RequiredScopes, cancellationToken);
            if (_options.CacheToken)
            {
                _tokenCache.SetAccessToken(token);
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token.Type, token.Value);
        }

        private Uri GetAccessTokenUrl(IReadOnlyCollection<string> requestedScopes)
        {
            var requiredScope = OAuth.Core.Models.Scope.Create(requestedScopes);
            var queryParameters = new Dictionary<string, string>()
            {
                { AuthorizationGrantType.GrantTypeParameter,                        AuthorizationGrantType.ClientCredentials.Type },
                { AuthorizationGrantType.ClientCredentials.ClientIdParameter,       _options.ClientId },
                { AuthorizationGrantType.ClientCredentials.ClientSecretParameter,   _options.ClientSecret },
                { ClaimTypes.Scope,                                                 requiredScope.Value }
            };

            return GetUrlWithQuery("oauth2/token", queryParameters);
        }

        private static Uri GetUrlWithQuery(string path, IDictionary<string, string> queryParameters)
        {
            return new Uri(new UriBuilder()
            {
                Path = path,
                Query = new QueryBuilder(queryParameters).ToQueryString().ToString()
            }.Uri.PathAndQuery, UriKind.Relative);
        }

        private static string GetUnauthorizedErrorDescription(HttpResponseHeaders headers)
        {
            const string MissingParametersHeader = "WWW-Authenticate";
            if (headers.TryGetValues(MissingParametersHeader, out var errorDescription))
            {
                return string.Join(',', errorDescription);
            }

            return "Unknown";
        }
    }
}
