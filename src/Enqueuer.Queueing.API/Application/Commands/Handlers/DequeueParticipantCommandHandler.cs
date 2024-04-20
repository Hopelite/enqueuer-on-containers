using Enqueuer.Queueing.Domain.Exceptions;
using Enqueuer.Queueing.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Enqueuer.Queueing.API.Application.Commands.Handlers;

public class DequeueParticipantCommandHandler : IOperationHandler<DequeueParticipantCommand>
{
    private readonly IGroupRepository _groupRepository;

    public DequeueParticipantCommandHandler(IGroupRepository groupRepository)
    {
        _groupRepository = groupRepository;
    }

    public async Task<IActionResult> Handle(DequeueParticipantCommand request, CancellationToken cancellationToken)
    {
        var group = await _groupRepository.GetOrCreateGroupAsync(request.GroupId, cancellationToken);
        try
        {
            group.DequeueParticipant(request.QueueName, request.ParticipantId);
        }
        catch (Exception ex) when (ex is QueueDoesNotExistException or ParticipantDoesNotExistException)
        {
            return new NotFoundObjectResult(ex.Message);
        }

        await _groupRepository.SaveChangesAsync(group, cancellationToken);
        return new OkResult();
    }
}
