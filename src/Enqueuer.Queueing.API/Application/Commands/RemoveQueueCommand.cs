using MediatR;

namespace Enqueuer.Queueing.API.Application.Commands;

public class RemoveQueueCommand : IRequest
{
    public RemoveQueueCommand(int id)
    {
        Id = id;
    }

    public int Id { get; }
}
