using Enqueuer.Queueing.Domain.Exceptions;
using Enqueuer.Queueing.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Enqueuer.Queueing.API.Application.Commands.Handlers;

public class EnqueueParticipantCommandHandler : IOperationHandler<EnqueueParticipantCommand>
{
    private readonly IGroupRepository _groupRepository;

    public EnqueueParticipantCommandHandler(IGroupRepository groupRepository)
    {
        _groupRepository = groupRepository;
    }

    public async Task<IActionResult> Handle(EnqueueParticipantCommand request, CancellationToken cancellationToken)
    {
        var group = await _groupRepository.GetOrCreateGroupAsync(request.GroupId, cancellationToken);
        try
        {
            group.EnqueueParticipant(request.QueueName, request.ParticipantId);
        }
        catch (QueueDoesNotExistException ex)
        {
            return new NotFoundObjectResult(ex.Message);
        }
        catch (Exception ex) when (ex is ParticipantAlreadyExistsException)
        {
            return new ConflictObjectResult(ex.Message);
        }

        await _groupRepository.SaveChangesAsync(group, cancellationToken);
        return new OkResult();
    }
}
