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
        var queue = _queueRepository.CreateNewQueue(request.QueueName, request.LocationId);

        await _queueRepository.SaveChangesAsync(cancellationToken);

        return new CreatedQueueViewModel(queue.Id);
    }
}
