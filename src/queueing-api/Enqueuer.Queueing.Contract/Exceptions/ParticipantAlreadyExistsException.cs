using System;

namespace Enqueuer.Queueing.Contract.V1.Exceptions
{
    public class ParticipantAlreadyExistsException : QueueingClientException
    {
        public ParticipantAlreadyExistsException()
        {
        }

        public ParticipantAlreadyExistsException(string message)
            : base(message)
        {
        }

        public ParticipantAlreadyExistsException(string message, Exception innerExcepton)
            : base(message, innerExcepton)
        {
        }
    }
}
