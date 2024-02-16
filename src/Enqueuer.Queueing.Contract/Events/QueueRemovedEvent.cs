namespace Enqueuer.Queueing.Contract.V1.Events
{
    public class QueueRemovedEvent : EventBase
    {
        public QueueRemovedEvent(long queueId, string queueName, long groupId, long onBehalfId, string onBehalfName)
        {
            QueueId = queueId;
            QueueName = queueName;
            GroupId = groupId;
            OnBehalfId = onBehalfId;
            OnBehalfName = onBehalfName;
        }

        public long QueueId { get; }

        public string QueueName { get; }

        public long GroupId { get; }

        public long OnBehalfId { get; }

        public string OnBehalfName { get; }
    }
}
