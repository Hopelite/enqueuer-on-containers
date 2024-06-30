using System;

namespace Enqueuer.Identity.Contract.V1.Models
{
    /// <summary>
    /// Contains the descriptive information about resource. 
    /// </summary>
    public class Metadata
    {
        internal Metadata(int maxAge)
            : this(TimeSpan.FromSeconds(maxAge))
        {
        }

        internal Metadata(TimeSpan maxAge)
        {
            MaxAge = maxAge;
        }

        /// <summary>
        /// The max time the resource should be cached to be considered actual. 
        /// </summary>
        public TimeSpan MaxAge { get; }
    }
}
