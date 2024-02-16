namespace Enqueuer.Queueing.Contract.V1.Commands
{
    public class DeleteGroupQueueCommand
    {
        public DeleteGroupQueueCommand(long groupId, string queueName)
        {
            GroupId = groupId;
            QueueName = queueName;
        }

        public long GroupId { get; }

        public string QueueName { get; }
    }
}
