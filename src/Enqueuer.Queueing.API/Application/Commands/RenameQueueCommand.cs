using MediatR;

namespace Enqueuer.Queueing.API.Application.Commands;

public class RenameQueueCommand : IRequest
{
    public RenameQueueCommand(int id, string newQueueName)
    {
        Id = id;
        NewQueueName = newQueueName;
    }

    public int Id { get; }

    public string NewQueueName { get; }
}
