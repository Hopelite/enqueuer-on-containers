using Enqueuer.Queueing.Domain.Models;

namespace Enqueuer.Queueing.Infrastructure.Commands;

public class CreateQueueCommand : ICommand
{
    public CreateQueueCommand(long groupId, string queueName)
    {
        GroupId = groupId;
        QueueName = queueName;
    }

    public long GroupId { get; }

    public string Name => nameof(CreateQueueCommand);

    public string QueueName { get; }

    public void Execute(Group group)
    {
        group.CreateQueue(QueueName);
    }
}
