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

    public Uri GetResourceId()
    {
        return new Uri($"groups/{GroupId}/queues/{QueueName}", UriKind.Relative);
    }
}
