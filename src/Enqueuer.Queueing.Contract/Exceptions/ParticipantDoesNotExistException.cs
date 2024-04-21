using System;

namespace Enqueuer.Queueing.Contract.V1.Exceptions
{
    public class ParticipantDoesNotExistException : QueueingClientException
    {
        public ParticipantDoesNotExistException()
        {
        }

        public ParticipantDoesNotExistException(string message)
            : base(message)
        {
        }

        public ParticipantDoesNotExistException(string message, Exception innerExcepton)
            : base(message, innerExcepton)
        {
        }
    }
}
