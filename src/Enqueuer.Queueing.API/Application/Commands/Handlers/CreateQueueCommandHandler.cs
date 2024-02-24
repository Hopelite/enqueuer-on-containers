using Enqueuer.Queueing.Domain.Exceptions;
using Enqueuer.Queueing.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Enqueuer.Queueing.API.Application.Commands.Handlers;

public class CreateQueueCommandHandler : IOperationHandler<CreateQueueCommand>
{
    private readonly IGroupRepository _groupRepository;

    public CreateQueueCommandHandler(IGroupRepository groupRepository)
    {
        _groupRepository = groupRepository;
    }

    public async Task<IActionResult> Handle(CreateQueueCommand request, CancellationToken cancellationToken)
    {
        var group = await _groupRepository.GetGroupAsync(request.GroupId, cancellationToken);

        try
        {
            group.CreateQueue(request.QueueName);
        }
        catch (InvalidQueueNameException ex)
        {
            return new BadRequestObjectResult(ex.Message);
        }
        catch (QueueAlreadyExistsException ex)
        {
            return new ConflictObjectResult(ex.Message);
        }

        await _groupRepository.SaveChangesAsync(group, cancellationToken);
        return new OkResult();
    }
}
