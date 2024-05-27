using Enqueuer.Identity.Contract.V1;
using Enqueuer.Identity.Contract.V1.Models;
using Enqueuer.Queueing.Domain.Models;
using Enqueuer.Queueing.Infrastructure.Commands.Handling;
using Microsoft.AspNetCore.Mvc;

namespace Enqueuer.Queueing.API.Application.Commands.Handlers;

public class CreateQueueCommandHandler : IOperationHandler<CreateQueueCommand>
{
    private readonly ICommandHandlerManager<Group> _commandHandlerManager;
    private readonly IIdentityClient _identityClient;

    public CreateQueueCommandHandler(ICommandHandlerManager<Group> commandHandlerManager, IIdentityClient identityClient)
    {
        _commandHandlerManager = commandHandlerManager;
        _identityClient = identityClient;
    }

    public async Task<IActionResult> Handle(CreateQueueCommand request, CancellationToken cancellationToken)
    {
        var checkAccessRequest = new CheckAccessRequest(request.GetResourceId(), request.CreatorId, AllowedScopes.QueueCreate);
        var isAbleToCreate = await _identityClient.CheckAccessAsync(checkAccessRequest, cancellationToken);

        if (!isAbleToCreate)
        {
            return new ForbidResult($"User '{request.CreatorId}' doesn't have rights to create queue in the group '{request.GroupId}'.");
        }

        var actualCommand = new Infrastructure.Commands.CreateQueueCommand(request.GroupId, request.QueueName);

        var handler = await _commandHandlerManager.GetActiveCommandHandlerForAsync(actualCommand.GroupId);

        await handler.HandleAsync(actualCommand, cancellationToken);

        return new AcceptedResult();
    }
}
