using Enqueuer.Queueing.Domain.Models;
using Enqueuer.Queueing.Infrastructure.Commands.Handling;
using Microsoft.AspNetCore.Mvc;

namespace Enqueuer.Queueing.API.Application.Commands.Handlers;

public class CreateQueueCommandHandler : IOperationHandler<CreateQueueCommand>
{
    private readonly ICommandHandlerManager<Group> _commandHandlerManager;

    public CreateQueueCommandHandler(ICommandHandlerManager<Group> commandHandlerManager)
    {
        _commandHandlerManager = commandHandlerManager;
    }

    public async Task<IActionResult> Handle(CreateQueueCommand request, CancellationToken cancellationToken)
    {
        var actualCommand = new Infrastructure.Commands.CreateQueueCommand(request.GroupId, request.QueueName);

        var handler = await _commandHandlerManager.GetActiveCommandHandlerForAsync(actualCommand.GroupId);

        await handler.HandleAsync(actualCommand, cancellationToken);

        return new AcceptedResult();
    }
}
