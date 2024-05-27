namespace Enqueuer.Queueing.API.Application.Commands;

public class CreateQueueCommand : IOperation
{
    public CreateQueueCommand(long groupId, string queueName, long creatorId)
    {
        GroupId = groupId;
        QueueName = queueName;
        CreatorId = creatorId;
    }

    public long GroupId { get; }

    public string QueueName { get; }
    public long CreatorId { get; }
}
