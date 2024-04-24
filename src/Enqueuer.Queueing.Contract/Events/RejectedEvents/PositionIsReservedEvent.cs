namespace Enqueuer.Queueing.Contract.V1.Events.RejectedEvents
{
    public class PositionIsReservedEvent : EventBase
    {
        public PositionIsReservedEvent(long groupId, string queueName, uint position)
        {
            GroupId = groupId;
            QueueName = queueName;
            Position = position;
        }

        public long GroupId { get; }

        public string QueueName { get; }

        public uint Position { get; }
    }
}
