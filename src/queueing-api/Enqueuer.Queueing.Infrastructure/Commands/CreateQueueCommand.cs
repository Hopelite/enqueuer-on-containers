using Enqueuer.Queueing.Domain.Models;

namespace Enqueuer.Queueing.Infrastructure.Commands;

public class CreateQueueCommand : ICommand
{
    public CreateQueueCommand(long groupId, string queueName, long creatorId)
    {
        GroupId = groupId;
        QueueName = queueName;
        CreatorId = creatorId;
    }

    public long GroupId { get; }

    public string Name => nameof(CreateQueueCommand);

    public string QueueName { get; }

    public long CreatorId { get; }

    public void Execute(Group group)
    {
        group.CreateQueue(QueueName, CreatorId);
    }
}
