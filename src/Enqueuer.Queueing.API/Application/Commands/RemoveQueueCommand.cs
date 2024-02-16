using MediatR;

namespace Enqueuer.Queueing.API.Application.Commands;

public class RemoveQueueCommand : IRequest
{
    public RemoveQueueCommand(long id)
    {
        Id = id;
    }

    public long Id { get; }
}
