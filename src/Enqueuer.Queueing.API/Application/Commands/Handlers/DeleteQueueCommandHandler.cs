using Enqueuer.Identity.Contract.V1;
using Enqueuer.Identity.Contract.V1.Models;
using Enqueuer.Identity.Contract.V1.Models.Enums;
using Enqueuer.Queueing.Domain.Models;
using Enqueuer.Queueing.Infrastructure.Commands.Handling;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace Enqueuer.Queueing.API.Application.Commands.Handlers;

public class DeleteQueueCommandHandler : IOperationHandler<DeleteQueueCommand>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IIdentityClient _identityClient;
    private readonly ICommandHandlerManager<Group> _commandHandlerManager;

    public DeleteQueueCommandHandler(
        IHttpContextAccessor httpContextAccessor,
        IIdentityClient identityClient,
        ICommandHandlerManager<Group> commandHandlerManager)
    {
        _httpContextAccessor = httpContextAccessor;
        _identityClient = identityClient;
        _commandHandlerManager = commandHandlerManager;
    }

    public async Task<IActionResult> Handle(DeleteQueueCommand request, CancellationToken cancellationToken)
    {
        var httpContext = _httpContextAccessor.HttpContext;

        var subClaim = httpContext.User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub);

        // TODO: consider creator_id to be a string, which may be either telegram user or an app
        if (subClaim != null)
        {
            var userId = long.Parse(subClaim.Value);
            var resourceId = new Uri($"groups/{request.GroupId}/queues/{request.QueueName}", UriKind.Relative);
            var hasAccess = await _identityClient.CheckAccessAsync(new CheckAccessRequest(resourceId, userId, AllowedScope.QueueDelete), cancellationToken);
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
