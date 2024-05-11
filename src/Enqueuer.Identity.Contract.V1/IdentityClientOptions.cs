using System;

namespace Enqueuer.Identity.Contract.V1
{
    public class IdentityClientOptions
    {
        public static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(10);
        public const int DefaultRetries = 3;

        public Uri BaseAddress { get; set; } = null!;

        public TimeSpan Timeout { get; set; } = DefaultTimeout;

        public int MaxRetries { get; set; } = DefaultRetries;

        /// <summary>
        /// Whether to cache the access tokens. Set to true by default.
        /// </summary>
        public bool CacheToken { get; set; } = true;
    }
}
