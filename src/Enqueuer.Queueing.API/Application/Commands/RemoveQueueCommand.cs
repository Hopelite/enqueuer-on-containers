namespace Enqueuer.Queueing.API.Application.Commands;

public class RemoveQueueCommand : IOperation
{
    public RemoveQueueCommand(long groupId, string queueName)
    {
        GroupId = groupId;
        QueueName = queueName;
    }

    public long GroupId { get; }

    public string QueueName { get; }
}
