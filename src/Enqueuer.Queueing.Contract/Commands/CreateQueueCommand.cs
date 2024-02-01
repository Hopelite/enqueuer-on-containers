namespace Enqueuer.Queueing.Contract.V1.Commands
{
    public class CreateQueueCommand
    {
        public CreateQueueCommand(string queueName, long locationId)
        {
            QueueName = queueName;
            LocationId = locationId;
        }

        public string QueueName { get; }

        public long LocationId { get; }
    }

}
