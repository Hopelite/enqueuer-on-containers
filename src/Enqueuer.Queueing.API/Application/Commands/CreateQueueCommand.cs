using MediatR;

namespace Enqueuer.Queueing.API.Application.Commands;

public class CreateQueueCommand : IRequest<int>
{
    public CreateQueueCommand(string queueName)
    {
        QueueName = queueName;
    }

    public string QueueName { get; }
}
