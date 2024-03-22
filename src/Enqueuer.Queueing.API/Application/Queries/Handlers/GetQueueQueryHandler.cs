using Enqueuer.Queueing.Contract.V1.Queries.ViewModels;
using Enqueuer.Queueing.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Enqueuer.Queueing.API.Application.Queries.Handlers;

public class GetQueueQueryHandler : IOperationHandler<GetGroupQueuesQuery>
{
    private readonly IGroupRepository _groupRepository;

    public GetQueueQueryHandler(IGroupRepository groupRepository)
    {
        _groupRepository = groupRepository;
    }

    public async Task<IActionResult> Handle(GetGroupQueuesQuery request, CancellationToken cancellationToken)
    {
        var group = await _groupRepository.GetGroupAsync(request.GroupId, cancellationToken);

        var queues = group.Queues.Select(q => new Queue(
            q.GroupId,
            q.Name,
            q.Participants.Select(p => new Participant(p.Id, p.Position.Number)).ToArray()));

        return new OkObjectResult(queues);
    }
}
