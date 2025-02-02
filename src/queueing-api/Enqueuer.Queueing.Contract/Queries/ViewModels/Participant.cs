namespace Enqueuer.Queueing.Contract.V1.Queries.ViewModels
{
    public class Participant
    {
        public Participant(long id, uint position)
        {
            Id = id;
            Position = position;
        }

        public long Id { get; }

        public uint Position { get; }
    }
}
