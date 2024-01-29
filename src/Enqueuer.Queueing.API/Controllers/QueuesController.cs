using Enqueuer.Queueing.API.Contract.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Enqueuer.Queueing.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QueuesController : ControllerBase
{
    [HttpGet("{id}")]
    public Task<IActionResult> GetQueue(int id, CancellationToken cancellationToken)
    {
        return Task.FromResult((IActionResult)Ok());
    }

    [HttpPost]
    public async Task<IActionResult> CreateQueue(
        [FromBody] CreateQueueCommand command,
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken)
    {
        var createQueueCommand = new Application.Commands.CreateQueueCommand(command.QueueName);

        var newQueueId = await mediator.Send(createQueueCommand, cancellationToken);

        return CreatedAtAction(nameof(GetQueue), routeValues: new { id = newQueueId }, value: new { id = newQueueId });
    }

    [HttpPut("{id}")]
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
