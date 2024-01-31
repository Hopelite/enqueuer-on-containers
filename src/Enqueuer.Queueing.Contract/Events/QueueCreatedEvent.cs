namespace Enqueuer.Queueing.Contract.V1.Events
{
    public class QueueCreatedEvent : EventBase
    {
        public QueueCreatedEvent(int queueId, string queueName, long locationId)
        {
            QueueId = queueId;
            QueueName = queueName;
            LocationId = locationId;
        }

        public override string Name => nameof(QueueCreatedEvent);

        public int QueueId { get; }

        public string QueueName { get; }

        public long LocationId { get; }
    }
}
