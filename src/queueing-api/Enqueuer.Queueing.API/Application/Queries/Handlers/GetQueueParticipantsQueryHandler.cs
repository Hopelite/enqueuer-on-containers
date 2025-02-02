using Enqueuer.Queueing.Contract.V1.Queries.ViewModels;
using Enqueuer.Queueing.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Enqueuer.Queueing.API.Application.Queries.Handlers;

public class GetQueueParticipantsQueryHandler : IOperationHandler<GetQueueParticipantsQuery>
{
    private readonly IGroupRepository _groupRepository;

    public GetQueueParticipantsQueryHandler(IGroupRepository groupRepository)
    {
        _groupRepository = groupRepository;
    }

    public async Task<IActionResult> Handle(GetQueueParticipantsQuery request, CancellationToken cancellationToken)
    {
        var group = await _groupRepository.GetOrCreateGroupAsync(request.GroupId, cancellationToken);

        var queue = group.Queues.FirstOrDefault(q => q.Name == request.QueueName);
        if (queue == null)
        {
            return new NotFoundResult();
        }

        var participants = queue.Participants.Select(p => new Participant(p.Id, p.Position.Number)).ToList();
        return new OkObjectResult(participants);
    }
}
