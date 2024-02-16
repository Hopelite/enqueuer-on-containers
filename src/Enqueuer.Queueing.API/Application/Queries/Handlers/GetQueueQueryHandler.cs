using Enqueuer.Queueing.Contract.V1.Queries.ViewModels;
using Enqueuer.Queueing.Domain.Repositories;
using MediatR;

namespace Enqueuer.Queueing.API.Application.Queries.Handlers;

public class GetQueueQueryHandler : IRequestHandler<GetQueueQuery, Queue>
{
    private readonly IQueueRepository _queueRepository;

    public GetQueueQueryHandler(IQueueRepository queueRepository)
    {
        _queueRepository = queueRepository;
    }

    public async Task<Queue> Handle(GetQueueQuery request, CancellationToken cancellationToken)
    {
        var queue = await _queueRepository.GetQueueAsync(request.Id, cancellationToken);

        return new Queue(
            queue.Id,
            queue.Name,
            queue.GroupId,
            queue.Participants.Select(p => new Participant(p.Id, p.Position.Number)).ToArray());
    }
}
