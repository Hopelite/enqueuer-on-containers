using Enqueuer.Queueing.Domain.Exceptions;
using Enqueuer.Queueing.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Enqueuer.Queueing.API.Application.Commands.Handlers;

public class DeleteQueueCommandHandler : IOperationHandler<DeleteQueueCommand>
{
    private readonly IGroupRepository _groupRepository;

    public DeleteQueueCommandHandler(IGroupRepository groupRepository)
    {
        _groupRepository = groupRepository;
    }

    public async Task<IActionResult> Handle(DeleteQueueCommand request, CancellationToken cancellationToken)
    {
        var group = await _groupRepository.GetOrCreateGroupAsync(request.GroupId, cancellationToken);
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
