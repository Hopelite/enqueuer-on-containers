using System;
using System.Collections.Generic;

namespace Enqueuer.Identity.Contract.V1.Models
{
    public class RevokeAccessRequest : ResourceAuthorizationRequest
    {
        public RevokeAccessRequest(Uri resourceId, long userId)
            : base(resourceId)
        {
            UserId = userId;
        }

        public long UserId { get; }

        internal IDictionary<string, string> GetQueryParameters()
        {
            return new Dictionary<string, string>()
            {
                { "user_id", UserId.ToString() }
            };
        }
    }
}
