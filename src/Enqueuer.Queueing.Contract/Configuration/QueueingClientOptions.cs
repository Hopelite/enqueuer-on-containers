using System;

namespace Enqueuer.Queueing.Contract.V1.Configuration
{
    public class QueueingClientOptions
    {
        private Uri _baseAddress;
        private string[] _requiredScopes;

        public Uri BaseAddress
        {
            get => _baseAddress;
            set => _baseAddress = value ?? throw new ArgumentNullException(nameof(BaseAddress));
        }

        public string[] RequiredScopes
        {
            get => _requiredScopes;
            set => _requiredScopes = value ?? throw new ArgumentNullException(nameof(_requiredScopes));
        }
    }
}
