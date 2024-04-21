using System;

namespace Enqueuer.Queueing.Contract.V1.Exceptions
{
    public class QueueDoesNotExistException : QueueingClientException
    {
        public QueueDoesNotExistException()
        {
        }

        public QueueDoesNotExistException(string message)
            : base(message)
        {
        }

        public QueueDoesNotExistException(string message, Exception innerExcepton)
            : base(message, innerExcepton)
        {
        }
    }
}
