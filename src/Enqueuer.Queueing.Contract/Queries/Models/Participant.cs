namespace Enqueuer.Queueing.Contract.V1.Queries.Models
{
    public class Participant
    {
        public Participant(long id, int queueId, uint position)
        {
            Id = id;
            QueueId = queueId;
            Position = position;
        }

        public long Id { get; }

        public int QueueId { get; }

        public uint Position { get; }
    }
}
