using Enqueuer.Queueing.API.Application.Commands;
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
    public Task<IActionResult> CreateQueue(CreateQueueCommand command, CancellationToken cancellationToken)
    {
        return Task.FromResult((IActionResult)Ok());
    }

    [HttpPut("{id}")]
    public Task<IActionResult> RenameQueue(int id, RenameQueueCommand command, CancellationToken cancellationToken)
    {
        return Task.FromResult((IActionResult)Ok());
    }

    [HttpDelete("{id}")]
    public Task<IActionResult> RemoveQueue(int id, CancellationToken cancellationToken)
    {
        return Task.FromResult((IActionResult)Ok());
    }

    // POST {queueId}/participants
    // Enqueue participant at the end of the queue

    // PUT {queueId}/participants/{position}
    // Enqueue participant at the specified position

    // DELETE: {queueId}/participants/{participantId}
    // Remove existing participant
    // TODO: Consider changing {participantId} to position
}
