using Enqueuer.Queueing.Domain.Exceptions;
using Enqueuer.Queueing.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Enqueuer.Queueing.API.Application.Commands.Handlers;

public class EnqueueParticipantAtCommandHandler : IOperationHandler<EnqueueParticipantAtCommand>
{
    private readonly IGroupRepository _groupRepository;

    public EnqueueParticipantAtCommandHandler(IGroupRepository groupRepository)
    {
        _groupRepository = groupRepository;
    }

    public async Task<IActionResult> Handle(EnqueueParticipantAtCommand request, CancellationToken cancellationToken)
    {
        var group = await _groupRepository.GetOrCreateGroupAsync(request.GroupId, cancellationToken);
        try
        {
            group.EnqueueParticipantAt(request.QueueName, request.ParticipantId, request.Position);
        }
        catch (QueueDoesNotExistException ex)
        {
            return new NotFoundObjectResult(ex.Message);
        }
        catch (Exception ex) when (ex is ParticipantAlreadyExistsException or PositionReservedException)
        {
            return new ConflictObjectResult(ex.Message);
        }

        await _groupRepository.SaveChangesAsync(group, cancellationToken);
        return new OkResult();
    }
}
