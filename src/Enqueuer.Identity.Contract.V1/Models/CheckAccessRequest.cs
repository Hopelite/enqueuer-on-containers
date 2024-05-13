using System;
using System.Collections.Generic;

namespace Enqueuer.Identity.Contract.V1.Models
{
    public class CheckAccessRequest
    {
        public CheckAccessRequest(Uri resourceId, long userId, string scope)
        {
            ResourceId = ValidateAndEncodeUri(resourceId);
            UserId = userId;
            Scope = ValidateScope(scope);
        }

        public Uri ResourceId { get; }

        public long UserId { get; }

        public string Scope { get; }

        internal IReadOnlyDictionary<string, string> GetQueryParameters()
        {
            return new Dictionary<string, string>()
            {
                { "user_id", UserId.ToString() },
                {   "scope", Scope }
            };
        }

        private static Uri ValidateAndEncodeUri(Uri resourceId)
        {
            if (resourceId == null)
            {
                throw new ArgumentNullException(nameof(resourceId));
            }

            return new Uri(Uri.EscapeDataString(resourceId.ToString()), UriKind.Relative);
        }

        private static string ValidateScope(string scope)
            => string.IsNullOrWhiteSpace(scope)
                ? throw new ArgumentNullException()
                : scope;
    }
}
