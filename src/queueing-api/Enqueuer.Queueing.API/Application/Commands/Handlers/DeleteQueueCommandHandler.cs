using Enqueuer.Identity.Contract.V1;
using Enqueuer.Identity.Contract.V1.Models;
using Enqueuer.Identity.Contract.V1.Models.Enums;
using Enqueuer.Queueing.API.Application.Claims;
using Enqueuer.Queueing.Domain.Models;
using Enqueuer.Queueing.Infrastructure.Commands.Handling;
using Microsoft.AspNetCore.Mvc;

namespace Enqueuer.Queueing.API.Application.Commands.Handlers;

public class DeleteQueueCommandHandler : IOperationHandler<DeleteQueueCommand>
{
    private readonly IClaimsAccessor _claimsAccessor;
    private readonly IIdentityClient _identityClient;
    private readonly ICommandHandlerManager<Group> _commandHandlerManager;

    public DeleteQueueCommandHandler(
        IClaimsAccessor claimsAccessor,
        IIdentityClient identityClient,
        ICommandHandlerManager<Group> commandHandlerManager)
    {
        _claimsAccessor = claimsAccessor;
        _identityClient = identityClient;
        _commandHandlerManager = commandHandlerManager;
    }

    public async Task<IActionResult> Handle(DeleteQueueCommand request, CancellationToken cancellationToken)
    {
        var userId = _claimsAccessor.GetUserIdFromClaims();
        if (userId.HasValue)
        {
            var resourceId = new Uri($"groups/{request.GroupId}/queues/{request.QueueName}", UriKind.Relative);
            var hasAccess = await _identityClient.CheckAccessAsync(new CheckAccessRequest(resourceId, userId.Value, AllowedScope.QueueDelete), cancellationToken);
            if (!hasAccess)
            {
                return new ForbidResult();
            }
        }

        var actualCommand = new Infrastructure.Commands.DeleteQueueCommand(request.GroupId, request.QueueName);

        var handler = await _commandHandlerManager.GetActiveCommandHandlerForAsync(actualCommand.GroupId);

        await handler.HandleAsync(actualCommand, cancellationToken);

        return new AcceptedResult();
    }
}
