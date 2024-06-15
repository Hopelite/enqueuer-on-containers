namespace Enqueuer.Queueing.Contract.V1.Events
{
    public class QueueDeletedEvent : EventBase
    {
        public QueueDeletedEvent(string queueName, long groupId, long onBehalfId)
        {
            QueueName = queueName;
            GroupId = groupId;
            OnBehalfId = onBehalfId;
        }

        public string QueueName { get; }

        public long GroupId { get; }

        public long OnBehalfId { get; }
    }
}
