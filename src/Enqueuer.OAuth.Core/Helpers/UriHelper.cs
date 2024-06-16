using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http.Extensions;

namespace Enqueuer.OAuth.Core.Helpers
{
    public static class UriHelper
    {
        public static Uri GetUriWithQuery(string path, IDictionary<string, string> queryParameters, UriKind uriKind)
        {
            var uri = new Uri(path, uriKind);
            return uri.AppendQuery(queryParameters);
        }

        /// <summary>
        /// Appends <paramref name="queryParameters"/> to the <paramref name="uri"/>.
        /// </summary>
        public static Uri AppendQuery(this Uri uri, IDictionary<string, string> queryParameters)
        {
            // TODO: consider to include uri parameter's query
            if (uri.IsAbsoluteUri)
            {
                return new UriBuilder(uri)
                {
                    Query = new QueryBuilder(queryParameters).ToQueryString().ToString()
                }.Uri;
            }

            return new Uri(new UriBuilder()
            {
                Path = uri.GetRelativeUriPath(),
                Query = new QueryBuilder(queryParameters).ToQueryString().ToString()
            }.Uri.PathAndQuery, UriKind.Relative);
        }

        private static string GetRelativeUriPath(this Uri uri)
        {
            var pathAndQuery = uri.OriginalString.Split('?');
            if (pathAndQuery.Length == 0)
            {
                throw new InvalidOperationException();
            }

            return pathAndQuery[0];
        }
    }
}
