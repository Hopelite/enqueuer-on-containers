using Enqueuer.Queueing.Domain.Repositories;
using MediatR;

namespace Enqueuer.Queueing.API.Application.Commands.Handlers;

public class RemoveQueueCommandHandler : IRequestHandler<RemoveQueueCommand>
{
    private readonly IQueueRepository _queueRepository;

    public RemoveQueueCommandHandler(IQueueRepository queueRepository)
    {
        _queueRepository = queueRepository;
    }

    public async Task Handle(RemoveQueueCommand request, CancellationToken cancellationToken)
    {
        var queue = await _queueRepository.GetQueueAsync(request.Id, cancellationToken);

        _queueRepository.DeleteQueue(queue);

        await _queueRepository.SaveChangesAsync(cancellationToken);
    }
}
