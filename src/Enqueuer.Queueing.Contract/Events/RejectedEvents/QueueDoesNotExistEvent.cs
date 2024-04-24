namespace Enqueuer.Queueing.Contract.V1.Events.RejectedEvents
{
    public class QueueDoesNotExistEvent : EventBase
    {
        public QueueDoesNotExistEvent(long groupId, string queueName)
        {
            GroupId = groupId;
            QueueName = queueName;
        }

        public long GroupId { get; set; }

        public string QueueName { get; set; }
    }
}
