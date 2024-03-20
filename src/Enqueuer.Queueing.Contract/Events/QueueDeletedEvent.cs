namespace Enqueuer.Queueing.Contract.V1.Events
{
    public class QueueDeletedEvent : EventBase
    {
        public QueueDeletedEvent(string queueName, long groupId, long onBehalfId, string onBehalfName)
        {
            QueueName = queueName;
            GroupId = groupId;
            OnBehalfId = onBehalfId;
            OnBehalfName = onBehalfName;
        }

        public string QueueName { get; }

        public long GroupId { get; }

        public long OnBehalfId { get; }

        public string OnBehalfName { get; }
    }
}
