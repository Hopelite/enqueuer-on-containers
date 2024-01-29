using MediatR;

namespace Enqueuer.Queueing.API.Application.Commands.Handlers;

public class RenameQueueCommandHandler : IRequestHandler<RenameQueueCommand>
{
    public Task Handle(RenameQueueCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
