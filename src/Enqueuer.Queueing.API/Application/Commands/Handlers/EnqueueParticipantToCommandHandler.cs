using Enqueuer.Queueing.Domain.Exceptions;
using Enqueuer.Queueing.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Enqueuer.Queueing.API.Application.Commands.Handlers;

public class EnqueueParticipantToCommandHandler : IOperationHandler<EnqueueParticipantToCommand>
{
    private readonly IGroupRepository _groupRepository;

    public EnqueueParticipantToCommandHandler(IGroupRepository groupRepository)
    {
        _groupRepository = groupRepository;
    }

    public async Task<IActionResult> Handle(EnqueueParticipantToCommand request, CancellationToken cancellationToken)
    {
        var group = await _groupRepository.GetOrCreateGroupAsync(request.GroupId, cancellationToken);
        try
        {
            group.EnqueueParticipantOn(request.QueueName, request.ParticipantId, request.Position);
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
