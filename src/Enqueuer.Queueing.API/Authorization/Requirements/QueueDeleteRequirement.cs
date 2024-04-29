using Microsoft.AspNetCore.Authorization;

namespace Enqueuer.Queueing.API.Authorization.Requirements;

public class QueueDeleteRequirement : IAuthorizationRequirement
{
}

public class QueueDeleteHandler : AuthorizationHandler<QueueDeleteRequirement>, IAuthorizationRequirement
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public QueueDeleteHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, QueueDeleteRequirement requirement)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null)
        {
            context.Fail();
            return Task.CompletedTask;
        }

        var routeData = httpContext.GetRouteData();
        var groupId = routeData.Values["groupId"]?.ToString();
        var queueName = routeData.Values["queueName"]?.ToString();

        if (groupId == null || queueName == null)
        {
            throw new InvalidOperationException();
        }

        var queueAccess = context.User.Claims.FirstOrDefault(c => c.Type == "queue_access")?.Value;
        var groupAccess = context.User.Claims.FirstOrDefault(c => c.Type == "group_access")?.Value;

        // Check if the user's claims include the specific queueName or groupId
        if ((queueAccess != null && queueAccess.Split(',').Contains(queueName)) ||
            (groupAccess != null && groupAccess.Split(',').Contains(groupId)))
        {
            context.Succeed(requirement);
        }
        else
        {
            context.Fail();
        }

        return Task.CompletedTask;
    }
}