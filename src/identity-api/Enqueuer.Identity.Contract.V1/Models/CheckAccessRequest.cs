using System;
using System.Collections.Generic;

namespace Enqueuer.Identity.Contract.V1.Models
{
    public class CheckAccessRequest : ResourceAuthorizationRequest
    {
        public CheckAccessRequest(Uri resourceId, long userId, string scope)
            : base(resourceId, userId)
        {
            Scope = ValidateScope(scope);
        }

        public string Scope { get; }

        internal IDictionary<string, string> GetQueryParameters()
        {
            return new Dictionary<string, string>()
            {
                { "user_id", UserId.ToString() },
                {   "scope", Scope }
            };
        }

        private static string ValidateScope(string scope)
            => string.IsNullOrWhiteSpace(scope)
                ? throw new ArgumentNullException()
                : scope;
    }
}
