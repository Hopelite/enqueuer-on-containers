using System;
using System.Collections.Generic;

namespace Enqueuer.Identity.Contract.V1.Models
{
    public class GrantAccessRequest : ResourceAuthorizationRequest
    {
        public GrantAccessRequest(Uri resourceId, long userId, string roleName)
            : base(resourceId, userId)
        {
            RoleName = ValidateRole(roleName);
        }

        public string RoleName { get; }

        internal IDictionary<string, string> GetQueryParameters()
        {
            return new Dictionary<string, string>()
            {
                { "user_id", UserId.ToString() },
                {    "role", RoleName }
            };
        }

        private static string ValidateRole(string roleName)
            => string.IsNullOrWhiteSpace(roleName)
                ? throw new ArgumentNullException(nameof(roleName))
                : roleName;
    }
}
