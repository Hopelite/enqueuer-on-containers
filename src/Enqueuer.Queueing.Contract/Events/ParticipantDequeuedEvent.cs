namespace Enqueuer.Queueing.Contract.V1.Events
{
    public class ParticipantDequeuedEvent : EventBase
    {
        public ParticipantDequeuedEvent(long groupId, string queueName, long participantId)
        {
            GroupId = groupId;
            QueueName = queueName;
            ParticipantId = participantId;
        }

        public long GroupId { get; }

        public string QueueName { get; }

        public long ParticipantId { get; }

        // TODO: possibly add position from which participant was dequeued
    }
}
