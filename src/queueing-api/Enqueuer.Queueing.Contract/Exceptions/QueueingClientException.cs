using System;

namespace Enqueuer.Queueing.Contract.V1.Exceptions
{
    public class QueueingClientException : Exception
    {
        public QueueingClientException()
        {
        }

        public QueueingClientException(string message)
            : base(message)
        {
        }

        public QueueingClientException(string message, Exception innerExcepton)
            : base(message, innerExcepton)
        {
        }
    }
}
