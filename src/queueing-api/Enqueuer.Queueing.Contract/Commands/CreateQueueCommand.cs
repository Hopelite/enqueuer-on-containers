namespace Enqueuer.Queueing.Contract.V1.Commands
{
    public class CreateQueueCommand
    {
        public CreateQueueCommand(long creatorId)
        {
            CreatorId = creatorId;
        }

        public long CreatorId { get; }
    }
}
