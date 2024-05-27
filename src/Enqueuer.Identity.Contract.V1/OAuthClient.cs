using Enqueuer.Identity.Contract.V1.Exceptions;
using Enqueuer.Identity.Contract.V1.Models;
using Microsoft.AspNetCore.Http.Extensions;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Enqueuer.Identity.Contract.V1
{
    public class OAuthClient : IOauthClient
    {
        private readonly HttpClient _httpClient;

        public OAuthClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task AuthorizeAsync(AuthorizationRequest request, CancellationToken cancellationToken)
        {
            var uri = GetUrlWithQuery("oauth2/authorize", request.GetQueryParameters());
            var response = await _httpClient.GetAsync(uri, cancellationToken);

            var responseBody = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new IdentityClientException();
            }

            throw new NotImplementedException();
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
