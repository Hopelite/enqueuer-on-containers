using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Enqueuer.Queueing.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GroupsController : ControllerBase
{
    private readonly IMediator _mediator;

    public GroupsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{groupId}/queues")]
    public Task<IActionResult> GetGroupQueues(long groupId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    [HttpPut("{groupId}/queues/{queueName}")]
    public async Task<IActionResult> CreateQueue(long groupId, string queueName, CancellationToken cancellationToken)
    {
        var createQueueCommand = new Application.Commands.CreateQueueCommand(queueName, groupId);

        await _mediator.Send(createQueueCommand, cancellationToken);
        
        return Ok();
    }

    [HttpDelete("{groupId}/queues/{queueName}")]
    public async Task<IActionResult> DeleteQueue(long groupId, string queueName, CancellationToken cancellationToken)
    {
        var removeQueueCommand = new Application.Commands.RemoveQueueCommand(groupId, queueName);

        await _mediator.Send(removeQueueCommand, cancellationToken);

        return Ok();
    }
}
