using Enqueuer.Queueing.Domain.Exceptions;
using Enqueuer.Queueing.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Enqueuer.Queueing.API.Application.Commands.Handlers;

public class RemoveQueueCommandHandler : IOperationHandler<RemoveQueueCommand>
{
    private readonly IGroupRepository _groupRepository;

    public RemoveQueueCommandHandler(IGroupRepository groupRepository)
    {
        _groupRepository = groupRepository;
    }

    public async Task<IActionResult> Handle(RemoveQueueCommand request, CancellationToken cancellationToken)
    {
        var group = await _groupRepository.GetGroupAsync(request.GroupId, cancellationToken);
        try
        {
            group.DeleteQueue(request.QueueName);
        }
        catch (QueueDoesNotExistException ex)
        {
            return new NotFoundObjectResult(ex.Message);
        }

        await _groupRepository.SaveChangesAsync(group, cancellationToken);
        return new OkResult();
    }
}
