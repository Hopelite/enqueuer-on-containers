using System;

namespace Enqueuer.Identity.Contract.V1.Models
{
    public abstract class ResourceAuthorizationRequest
    {
        protected ResourceAuthorizationRequest(Uri resourceId)
        {
            ResourceId = ValidateAndEncodeUri(resourceId);
        }

        public Uri ResourceId { get; }

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
