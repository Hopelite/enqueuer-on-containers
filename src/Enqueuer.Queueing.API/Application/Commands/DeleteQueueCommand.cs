namespace Enqueuer.Queueing.API.Application.Commands;

public class DeleteQueueCommand : IOperation
{
    public DeleteQueueCommand(long groupId, string queueName)
    {
        GroupId = groupId;
        QueueName = queueName;
    }

    public long GroupId { get; }

    public string QueueName { get; }
}
