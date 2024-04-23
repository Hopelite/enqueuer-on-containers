using Enqueuer.Queueing.Domain.Models;
using Enqueuer.Queueing.Infrastructure.Commands.Handling;
using Microsoft.AspNetCore.Mvc;

namespace Enqueuer.Queueing.API.Application.Commands.Handlers;

public class EnqueueParticipantCommandHandler : IOperationHandler<EnqueueParticipantCommand>
{
    private readonly ICommandHandlerManager<Group> _commandHandlerManager;

    public EnqueueParticipantCommandHandler(ICommandHandlerManager<Group> commandHandlerManager)
    {
        _commandHandlerManager = commandHandlerManager;
    }

    public async Task<IActionResult> Handle(EnqueueParticipantCommand request, CancellationToken cancellationToken)
    {
        var actualCommand = new Infrastructure.Commands.EnqueueParticipantCommand(request.GroupId, request.QueueName, request.ParticipantId);

        var handler = await _commandHandlerManager.GetActiveCommandHandlerForAsync(actualCommand.GroupId);

        await handler.HandleAsync(actualCommand, cancellationToken);

        return new AcceptedResult();
    }
}
