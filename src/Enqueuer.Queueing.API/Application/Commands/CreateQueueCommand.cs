using MediatR;

namespace Enqueuer.Queueing.API.Application.Commands;

public class CreateQueueCommand : IRequest<int>
{
    public CreateQueueCommand(string queueName, long locationId)
    {
        QueueName = queueName;
        LocationId = locationId;
    }

    public string QueueName { get; }

    public long LocationId { get; }
}
