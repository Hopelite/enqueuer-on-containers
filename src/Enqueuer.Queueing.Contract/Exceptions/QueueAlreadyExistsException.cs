using System;

namespace Enqueuer.Queueing.Contract.V1.Exceptions
{
    public class QueueAlreadyExistsException : QueueingClientException
    {
        public QueueAlreadyExistsException()
        {
        }

        public QueueAlreadyExistsException(string message)
            : base(message)
        {
        }

        public QueueAlreadyExistsException(string message, Exception innerExcepton)
            : base(message, innerExcepton)
        {
        }
    }
}
