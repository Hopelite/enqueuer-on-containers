using MediatR;

namespace Enqueuer.Queueing.API.Application.Commands;

public class RemoveQueueCommand : IRequest
{
    public RemoveQueueCommand(long groupId, string queueName)
    {
        GroupId = groupId;
        QueueName = queueName;
    }

    public long GroupId { get; }

    public string QueueName { get; }
}
