using System;

namespace Enqueuer.Queueing.Contract.V1.Exceptions
{
    public class InvalidQueueNameException : QueueingClientException
    {
        public InvalidQueueNameException()
        {
        }

        public InvalidQueueNameException(string message)
            : base(message)
        {
        }

        public InvalidQueueNameException(string message, Exception innerExcepton)
            : base(message, innerExcepton)
        {
        }
    }
}
