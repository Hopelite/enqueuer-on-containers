namespace Enqueuer.Queueing.Contract.V1.Events
{
    public class ParticipantEnqueuedEvent : EventBase
    {
        public ParticipantEnqueuedEvent(long groupId, string queueName, long participantId, uint position)
        {
            GroupId = groupId;
            QueueName = queueName;
            ParticipantId = participantId;
            Position = position;
        }

        public override string Name => nameof(ParticipantEnqueuedEvent);

        public long GroupId { get; }

        public string QueueName { get; }

        public long ParticipantId { get; }

        public uint Position { get; }
    }
}
