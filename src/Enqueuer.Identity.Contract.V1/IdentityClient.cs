using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Enqueuer.Identity.Contract.V1.Exceptions;
using Enqueuer.Identity.Contract.V1.Models;
using Enqueuer.Identity.Contract.V1.OAuth.Exceptions;
using Microsoft.AspNetCore.Http.Extensions;

namespace Enqueuer.Identity.Contract.V1
{
    public class IdentityClient : IIdentityClient
    {
        private readonly HttpClient _httpClient;

        public IdentityClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> CheckAccessAsync(CheckAccessRequest request, CancellationToken cancellationToken)
        {
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
            var uri = new Uri($"api/authorization/users/{userId}", UriKind.Relative);

            var response = await _httpClient.GetAsync(uri, cancellationToken);
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new InvalidCredentialsException($"Authorization error. Reason: {GetUnauthorizedErrorDescription(response.Headers)}");
            }

            if (!response.IsSuccessStatusCode)
            {
                // Error
            }

            var responseBody = await response.Content.ReadAsStringAsync();
            var userInfo = JsonSerializer.Deserialize<GetUserInfoResponse>(responseBody, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower });
            if (userInfo == null)
            {
                // TODO: extract to single method
                throw new IdentityClientException("Unable to deserialize the GetUserInfo response.");
            }

            var cachingHeader = response.Headers.Age;
            var metadata = !cachingHeader.HasValue || cachingHeader.Value.Seconds <= 0
                ? new Metadata(0)
                : new Metadata(cachingHeader.Value);

            return new UserInfo(userInfo.UserId, userInfo.FirstName, userInfo.LastName, metadata);
        }

        public async Task CreateOrUpdateUserAsync(CreateOrUpdateUserRequest request, CancellationToken cancellationToken)
        {
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
