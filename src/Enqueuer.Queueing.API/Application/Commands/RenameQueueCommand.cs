using MediatR;

namespace Enqueuer.Queueing.API.Application.Commands;

public class RenameQueueCommand : IRequest
{
    public RenameQueueCommand(long id, string newQueueName)
    {
        Id = id;
        NewQueueName = newQueueName;
    }

    public long Id { get; }

    public string NewQueueName { get; }
}
