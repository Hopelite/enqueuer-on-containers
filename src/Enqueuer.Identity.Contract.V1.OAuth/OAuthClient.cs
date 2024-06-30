using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Enqueuer.Identity.Contract.V1.OAuth.Exceptions;
using Enqueuer.Identity.Contract.V1.OAuth.Models;
using Enqueuer.OAuth.Core.Helpers;
using Enqueuer.OAuth.Core.Tokens;

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
            var uri = UriHelper.GetUriWithQuery("oauth2/token", request.GetQueryParameters(), UriKind.Relative);
            var response = await _httpClient.PostAsync(uri, content: null, cancellationToken);

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
    }
}
