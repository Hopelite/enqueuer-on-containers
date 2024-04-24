namespace Enqueuer.Queueing.Contract.V1.Events.RejectedEvents
{
    public class RejectedEvent : EventBase
    {
        public RejectedEvent(long aggregateId, string errorMessage)
        {
            AggregateId = aggregateId;
            ErrorMessage = errorMessage;
        }

        public long AggregateId { get; set; }

        public string ErrorMessage { get; }
    }
}
