namespace Enqueuer.Queueing.API.Application.Commands;

public class CreateQueueCommand : IOperation
{
    public CreateQueueCommand(long groupId, string queueName)
    {
        GroupId = groupId;
        QueueName = queueName;
    }

    public long GroupId { get; }

    public string QueueName { get; }
}
