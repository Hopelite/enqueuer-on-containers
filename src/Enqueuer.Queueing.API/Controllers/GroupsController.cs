using Enqueuer.Queueing.Contract.V1.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Enqueuer.Queueing.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GroupsController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("{groupId}/queues")]
    public Task<IActionResult> GetGroupQueues(long groupId, CancellationToken cancellationToken)
    {
        var getGroupQueuesQuery = new Application.Queries.GetGroupQueuesQuery(groupId);
        return _mediator.Send(getGroupQueuesQuery, cancellationToken);
    }

    [AllowedScopes("queue:create", "queue", "group")]
    [HttpPut("{groupId}/queues/{queueName}")]
    public Task<IActionResult> CreateQueue(long groupId, string queueName, CreateQueueCommand command, CancellationToken cancellationToken)
    {
        var createQueueCommand = new Application.Commands.CreateQueueCommand(groupId, queueName, command.CreatorId);
        return _mediator.Send(createQueueCommand, cancellationToken);
    }

    [AllowedScopes("queue:delete", "queue", "group")]
    [HttpDelete("{groupId}/queues/{queueName}")]
    public Task<IActionResult> DeleteQueue(long groupId, string queueName, CancellationToken cancellationToken)
    {
        var removeQueueCommand = new Application.Commands.DeleteQueueCommand(groupId, queueName);
        return _mediator.Send(removeQueueCommand, cancellationToken);
    }

    // TODO: consider to add pagination
    [HttpGet("{groupId}/queues/{queueName}/participants")]
    public Task<IActionResult> GetQueueParticipants(long groupId, string queueName, CancellationToken cancellationToken)
    {
        var getQueueParticipantsQuery = new Application.Queries.GetQueueParticipantsQuery(groupId, queueName);
        return _mediator.Send(getQueueParticipantsQuery, cancellationToken);
    }

    [HttpPost("{groupId}/queues/{queueName}/participants")]
    public Task<IActionResult> EnqueueParticipant(long groupId, string queueName, EnqueueParticipantCommand command, CancellationToken cancellationToken)
    {
        var enqueueCommand = new Application.Commands.EnqueueParticipantCommand(groupId, queueName, command.ParticipantId);
        return _mediator.Send(enqueueCommand, cancellationToken);
    }

    [HttpPut("{groupId}/queues/{queueName}/participants/{position}")]
    public Task<IActionResult> EnqueueParticipantTo(long groupId, string queueName, uint position, EnqueueParticipantAtCommand command, CancellationToken cancellationToken)
    {
        var enqueueCommand = new Application.Commands.EnqueueParticipantAtCommand(groupId, queueName, command.ParticipantId, position);
        return _mediator.Send(enqueueCommand, cancellationToken);
    }

    [HttpDelete("{groupId}/queues/{queueName}/participants")]
    public Task<IActionResult> DequeueParticipant(long groupId, string queueName, DequeueParticipantCommand command, CancellationToken cancellationToken)
    {
        var dequeueCommand = new Application.Commands.DequeueParticipantCommand(groupId, queueName, command.ParticipantId);
        return _mediator.Send(dequeueCommand, cancellationToken);
    }
}
