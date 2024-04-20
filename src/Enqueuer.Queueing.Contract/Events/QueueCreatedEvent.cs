namespace Enqueuer.Queueing.Contract.V1.Events
{
    public class QueueCreatedEvent : EventBase
    {
        //public QueueCreatedEvent(long groupId, string queueName, long creatorId)
        //{
        //    CreatorId = creatorId;
        //    QueueName = queueName;
        //    GroupId = groupId;
        //}

        public long GroupId { get; set; }

        public string QueueName { get; set; }

        public long CreatorId { get; set; }
    }
}
