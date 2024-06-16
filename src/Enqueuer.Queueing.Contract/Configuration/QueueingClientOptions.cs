using System;
using Enqueuer.Identity.Contract.V1.OAuth.Configuration;

namespace Enqueuer.Queueing.Contract.V1.Configuration
{
    public class QueueingClientOptions : ClientCredentialsAuthorizationOptions<IQueueingClient>
    {
        public Uri BaseAddress { get; set; } = null!;
    }
}
