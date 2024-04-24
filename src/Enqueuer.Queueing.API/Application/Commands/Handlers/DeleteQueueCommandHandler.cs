using Enqueuer.Queueing.Domain.Models;
using Enqueuer.Queueing.Infrastructure.Commands.Handling;
using Microsoft.AspNetCore.Mvc;

namespace Enqueuer.Queueing.API.Application.Commands.Handlers;

public class DeleteQueueCommandHandler : IOperationHandler<DeleteQueueCommand>
{
    private readonly ICommandHandlerManager<Group> _commandHandlerManager;

    public DeleteQueueCommandHandler(ICommandHandlerManager<Group> commandHandlerManager)
    {
        _commandHandlerManager = commandHandlerManager;
    }

    public async Task<IActionResult> Handle(DeleteQueueCommand request, CancellationToken cancellationToken)
    {
        var actualCommand = new Infrastructure.Commands.DeleteQueueCommand(request.GroupId, request.QueueName);

        var handler = await _commandHandlerManager.GetActiveCommandHandlerForAsync(actualCommand.GroupId);

        await handler.HandleAsync(actualCommand, cancellationToken);

        return new AcceptedResult();
    }
}
