using System;

namespace Enqueuer.Queueing.Contract.V1.Exceptions
{
    public class ResourceAlreadyExistsException : QueueingClientException
    {
        public ResourceAlreadyExistsException()
        {
        }

        public ResourceAlreadyExistsException(string message)
            : base(message)
        {
        }

        public ResourceAlreadyExistsException(string message, Exception innerExcepton)
            : base(message, innerExcepton)
        {
        }
    }
}
