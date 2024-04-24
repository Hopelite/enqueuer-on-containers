using Enqueuer.Queueing.Domain.Models;
using Enqueuer.Queueing.Infrastructure.Commands.Handling;
using Microsoft.AspNetCore.Mvc;

namespace Enqueuer.Queueing.API.Application.Commands.Handlers;

public class EnqueueParticipantAtCommandHandler : IOperationHandler<EnqueueParticipantAtCommand>
{
    private readonly ICommandHandlerManager<Group> _commandHandlerManager;

    public EnqueueParticipantAtCommandHandler(ICommandHandlerManager<Group> commandHandlerManager)
    {
        _commandHandlerManager = commandHandlerManager;
    }

    public async Task<IActionResult> Handle(EnqueueParticipantAtCommand request, CancellationToken cancellationToken)
    {
        var actualCommand = new Infrastructure.Commands.EnqueueParticipantAtCommand(request.GroupId, request.QueueName, request.ParticipantId, request.Position);

        var handler = await _commandHandlerManager.GetActiveCommandHandlerForAsync(actualCommand.GroupId);

        await handler.HandleAsync(actualCommand, cancellationToken);

        return new AcceptedResult();
    }
}
