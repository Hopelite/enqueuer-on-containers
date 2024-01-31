using Enqueuer.Queueing.Contract.V1.Commands.ViewModels;
using Enqueuer.Queueing.Domain.Repositories;
using MediatR;

namespace Enqueuer.Queueing.API.Application.Commands.Handlers;

public class CreateQueueCommandHandler : IRequestHandler<CreateQueueCommand, CreatedQueueViewModel>
{
    private readonly IQueueRepository _queueRepository;

    public CreateQueueCommandHandler(IQueueRepository queueRepository)
    {
        _queueRepository = queueRepository;
    }

    public async Task<CreatedQueueViewModel> Handle(CreateQueueCommand request, CancellationToken cancellationToken)
    {
        var queue = await _queueRepository.CreateNewQueueAsync(request.QueueName, request.LocationId, cancellationToken);

        await _queueRepository.SaveChangesAsync(cancellationToken);

        return new CreatedQueueViewModel(queue.Id);
    }
}
