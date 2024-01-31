using Enqueuer.Queueing.Domain.Repositories;
using MediatR;

namespace Enqueuer.Queueing.API.Application.Commands.Handlers;

public class RenameQueueCommandHandler : IRequestHandler<RenameQueueCommand>
{
    private readonly IQueueRepository _queueRepository;

    public RenameQueueCommandHandler(IQueueRepository queueRepository)
    {
        _queueRepository = queueRepository;
    }

    public async Task Handle(RenameQueueCommand request, CancellationToken cancellationToken)
    {
        var queue = await _queueRepository.GetQueueAsync(request.Id, cancellationToken);

        queue.ChangeName(request.NewQueueName);

        _queueRepository.UpdateQueue(queue);

        await _queueRepository.SaveChangesAsync(cancellationToken);
    }
}
