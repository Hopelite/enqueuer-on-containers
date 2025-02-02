using System;

namespace Enqueuer.Queueing.Contract.V1.Exceptions
{
    public class PositionReservedException : QueueingClientException
    {
        public PositionReservedException()
        {
        }

        public PositionReservedException(string message)
            : base(message)
        {
        }

        public PositionReservedException(string message, Exception innerExcepton)
            : base(message, innerExcepton)
        {
        }
    }
}
