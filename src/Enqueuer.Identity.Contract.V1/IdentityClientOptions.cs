using System;
using Enqueuer.Identity.Contract.V1.OAuth.Configuration;

namespace Enqueuer.Identity.Contract.V1
{
    public class IdentityClientOptions : ClientCredentialsAuthorizationOptions<IIdentityClient>
    {
        public static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(10);
        public const int DefaultRetries = 3;

        public Uri BaseAddress { get; set; } = null!;

        public TimeSpan Timeout { get; set; } = DefaultTimeout;

        public int MaxRetries { get; set; } = DefaultRetries;
    }
}
