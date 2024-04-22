using Enqueuer.Queueing.Domain.Models;

namespace Enqueuer.Queueing.Infrastructure.Commands;

public class DeleteQueueCommand : ICommand
{
    public DeleteQueueCommand(long groupId, string queueName)
    {
        GroupId = groupId;
        QueueName = queueName;
    }

    public long GroupId { get; }

    public string Name => nameof(DeleteQueueCommand);

    public string QueueName { get; }

    public void Execute(Group group)
    {
        group.DeleteQueue(QueueName);
    }
}
