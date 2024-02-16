namespace Enqueuer.Queueing.Contract.V1.Events
{
    public class QueueCreatedEvent : EventBase
    {
        public QueueCreatedEvent(int queueId, string queueName, long groupId)
        {
            QueueId = queueId;
            QueueName = queueName;
            GroupId = groupId;
        }

        public int QueueId { get; }

        public string QueueName { get; }

        public long GroupId { get; }
    }
}
