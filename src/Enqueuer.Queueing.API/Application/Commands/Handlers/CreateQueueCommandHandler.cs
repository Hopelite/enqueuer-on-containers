using Enqueuer.Identity.Contract.V1;
using Enqueuer.Identity.Contract.V1.Models;
using Enqueuer.Identity.Contract.V1.Models.Enums;
using Enqueuer.Queueing.Domain.Models;
using Enqueuer.Queueing.Infrastructure.Commands.Handling;
using Microsoft.AspNetCore.Mvc;

namespace Enqueuer.Queueing.API.Application.Commands.Handlers;

public class CreateQueueCommandHandler : IOperationHandler<CreateQueueCommand>
{
    private readonly IIdentityClient _identityClient;
    private readonly ICommandHandlerManager<Group> _commandHandlerManager;

    public CreateQueueCommandHandler(IIdentityClient identityClient, ICommandHandlerManager<Group> commandHandlerManager)
    {
        _identityClient = identityClient;
        _commandHandlerManager = commandHandlerManager;
    }

    public async Task<IActionResult> Handle(CreateQueueCommand request, CancellationToken cancellationToken)
    {
        var resourceId = new Uri($"groups/{request.GroupId}/queues", UriKind.Relative);
        var hasAccess = await _identityClient.CheckAccessAsync(new CheckAccessRequest(resourceId, request.CreatorId, AllowedScope.QueueCreate), cancellationToken);
        if (!hasAccess)
        {
            return new ForbidResult();
        }

        var actualCommand = new Infrastructure.Commands.CreateQueueCommand(request.GroupId, request.QueueName, request.CreatorId);

        var handler = await _commandHandlerManager.GetActiveCommandHandlerForAsync(actualCommand.GroupId);

        await handler.HandleAsync(actualCommand, cancellationToken);

        return new AcceptedResult();
    }
}
