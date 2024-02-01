using Enqueuer.Queueing.API.Application.Queries;
using Enqueuer.Queueing.Contract.V1.Commands;
using Enqueuer.Queueing.Contract.V1.Queries.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Enqueuer.Queueing.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QueuesController : ControllerBase
{
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Queue))]
    public async Task<IActionResult> GetQueue(
        [FromRoute] int id,
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken)
    {
        var getQueueQuery = new GetQueueQuery(id);

        var requestedQueue = await mediator.Send(getQueueQuery, cancellationToken);

        return Ok(requestedQueue);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateQueue(
        [FromBody] CreateQueueCommand command,
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken)
    {
        var createQueueCommand = new Application.Commands.CreateQueueCommand(command.QueueName, command.LocationId);

        var newQueueId = await mediator.Send(createQueueCommand, cancellationToken);

        return CreatedAtAction(nameof(GetQueue), routeValues: new { id = newQueueId }, value: new { id = newQueueId });
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> RenameQueue(
        [FromRoute] int id,
        [FromBody] RenameQueueCommand command,
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken)
    {
        var renameQueueCommand = new Application.Commands.RenameQueueCommand(id, command.NewQueueName);

        await mediator.Send(renameQueueCommand, cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> RemoveQueue(
        [FromRoute] int id,
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken)
    {
        var deleteQueueCommand = new Application.Commands.RemoveQueueCommand(id);

        await mediator.Send(deleteQueueCommand, cancellationToken);

        return NoContent();
    }

    // POST {queueId}/participants
    // Enqueue participant at the end of the queue

    // PUT {queueId}/participants/{position}
    // Enqueue participant at the specified position

    // DELETE: {queueId}/participants/{participantId}
    // Remove existing participant
    // TODO: Consider changing {participantId} to position
}
