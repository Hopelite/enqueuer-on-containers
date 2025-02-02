namespace Enqueuer.Queueing.Contract.V1.Events.RejectedEvents
{
    public class QueueAlreadyExistsEvent : EventBase
    {
        public QueueAlreadyExistsEvent(long groupId, string queueName)
        {
            GroupId = groupId;
            QueueName = queueName;
        }

        public override string Name => nameof(QueueAlreadyExistsEvent);

        public long GroupId { get; set; }

        public string QueueName { get; set; }

        // TODO: possibly add OnBehalfOf
    }
}
