using System;

namespace Enqueuer.Identity.Contract.V1.Models
{
    public abstract class ResourceAuthorizationRequest
    {
        protected ResourceAuthorizationRequest(Uri resourceId, long userId)
        {
            ResourceId = ValidateAndEncodeUri(resourceId);
            UserId = userId;
        }

        public Uri ResourceId { get; }

        public long UserId { get; }

        private static Uri ValidateAndEncodeUri(Uri resourceId)
        {
            if (resourceId == null)
            {
                throw new ArgumentNullException(nameof(resourceId));
            }

            return new Uri(Uri.EscapeDataString(resourceId.ToString()), UriKind.Relative);
        }
    }
}
