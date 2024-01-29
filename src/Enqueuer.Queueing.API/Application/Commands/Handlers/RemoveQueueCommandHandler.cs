using MediatR;

namespace Enqueuer.Queueing.API.Application.Commands.Handlers;

public class RemoveQueueCommandHandler : IRequestHandler<RemoveQueueCommand>
{
    public Task Handle(RemoveQueueCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
