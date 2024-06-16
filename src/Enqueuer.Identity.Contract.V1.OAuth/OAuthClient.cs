using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Enqueuer.Identity.Contract.V1.OAuth.Exceptions;
using Enqueuer.Identity.Contract.V1.OAuth.Models;
using Enqueuer.OAuth.Core.Tokens;
using Microsoft.AspNetCore.Http.Extensions;

namespace Enqueuer.Identity.Contract.V1.OAuth
{
    public class OAuthClient : IOAuthClient
    {
        private static readonly JsonSerializerOptions SerializerOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower };
        private readonly HttpClient _httpClient;

        public OAuthClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<AccessToken> GetAccessTokenAsync(IAccessTokenRequest request, CancellationToken cancellationToken)
        {
            var response = await _httpClient.PostAsync(GetUrlWithQuery("oauth2/token", request.GetQueryParameters()), content: null, cancellationToken);

            var responseBody = await response.Content.ReadAsStringAsync();
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new InvalidCredentialsException($"The provided credentials are invalid. Reason: {responseBody}");
            }

            if (!response.IsSuccessStatusCode)
            {
                throw new OAuthClientException($"The request to get access token was not successful. Reason: {response.StatusCode}, {responseBody}");
            }

            var tokenResponse = JsonSerializer.Deserialize<GetAccessTokenResponse>(responseBody, SerializerOptions);
            if (tokenResponse == null)
            {
                throw new OAuthClientException("Unable to deserialize the GetAccessToken response.");
            }

            return new AccessToken(tokenResponse.AccessToken, tokenResponse.TokenType, TimeSpan.FromSeconds(tokenResponse.ExpiresIn));
        }

        private static Uri GetUrlWithQuery(string path, IDictionary<string, string> queryParameters)
        {
            return new Uri(new UriBuilder()
            {
                Path = path,
                Query = new QueryBuilder(queryParameters).ToQueryString().ToString()
            }.Uri.PathAndQuery, UriKind.Relative);
        }
    }
}
