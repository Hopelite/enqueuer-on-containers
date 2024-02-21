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
        var getGroupQueuesQuery = new Application.Queries.GetGroupQueuesQuery(groupId);
        return _mediator.Send(getGroupQueuesQuery, cancellationToken);
    }

    [HttpPut("{groupId}/queues/{queueName}")]
    public Task<IActionResult> CreateQueue(long groupId, string queueName, CancellationToken cancellationToken)
    {
        var createQueueCommand = new Application.Commands.CreateQueueCommand(groupId, queueName);
        return _mediator.Send(createQueueCommand, cancellationToken);
    }

    [HttpDelete("{groupId}/queues/{queueName}")]
    public Task<IActionResult> DeleteQueue(long groupId, string queueName, CancellationToken cancellationToken)
    {
        var removeQueueCommand = new Application.Commands.RemoveQueueCommand(groupId, queueName);
        return _mediator.Send(removeQueueCommand, cancellationToken);
    }
}
