namespace Enqueuer.Queueing.Contract.V1.Commands
{
    public class RenameQueueCommand
    {
        public RenameQueueCommand(string newQueueName)
        {
            NewQueueName = newQueueName;
        }

        public string NewQueueName { get; }
    }
}
