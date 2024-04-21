namespace Enqueuer.Queueing.Contract.V1.Events
{
    public class ParticipantEnqueuedEvent : EventBase
    {
        public ParticipantEnqueuedEvent(long groupId, string queueName, long participantId)
        {
            GroupId = groupId;
            QueueName = queueName;
            ParticipantId = participantId;
        }

        public long GroupId { get; }

        public string QueueName { get; }

        public long ParticipantId { get; }
    }
}
