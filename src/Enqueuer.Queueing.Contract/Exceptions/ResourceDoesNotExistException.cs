using System;

namespace Enqueuer.Queueing.Contract.V1.Exceptions
{
    public class ResourceDoesNotExistException : QueueingClientException
    {
        public ResourceDoesNotExistException()
        {
        }

        public ResourceDoesNotExistException(string message)
            : base(message)
        {
        }

        public ResourceDoesNotExistException(string message, Exception innerExcepton)
            : base(message, innerExcepton)
        {
        }
    }
}
