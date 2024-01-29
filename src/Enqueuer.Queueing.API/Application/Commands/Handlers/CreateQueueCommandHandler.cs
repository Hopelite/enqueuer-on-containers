using MediatR;

namespace Enqueuer.Queueing.API.Application.Commands.Handlers;

public class CreateQueueCommandHandler : IRequestHandler<CreateQueueCommand, int>
{
    public Task<int> Handle(CreateQueueCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
