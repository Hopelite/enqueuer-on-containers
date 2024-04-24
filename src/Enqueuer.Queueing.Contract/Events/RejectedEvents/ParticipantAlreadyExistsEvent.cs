namespace Enqueuer.Queueing.Contract.V1.Events.RejectedEvents
{
    public class ParticipantAlreadyExistsEvent : EventBase
    {
        public ParticipantAlreadyExistsEvent(long groupId, string queueName, long participantId)
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
