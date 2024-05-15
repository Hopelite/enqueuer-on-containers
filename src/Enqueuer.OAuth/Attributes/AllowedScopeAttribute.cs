using Enqueuer.OAuth.Core.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Microsoft.AspNetCore.Authorization;

/// <summary>
/// Specifies that the class or method that this attribute is applied to requires the authorization by scope.
/// </summary>
public class AllowedScopeAttribute : AuthorizeAttribute, IAuthorizationFilter
{
    public AllowedScopeAttribute()
        : this(Array.Empty<string>())
    {
    }

    public AllowedScopeAttribute(params string[] allowedScope)
    {
        AllowedScope = allowedScope;
    }

    public string[] AllowedScope { get; init; }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var hasAllowedScope = false;

        var user = context.HttpContext.User;
        if (user.Identity != null && user.Identity.IsAuthenticated)
        {
            var scopeClaim = user.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Scope));
            if (scopeClaim != null)
            {
                var providedScope = ScopeClaim.Create(scopeClaim.Value);
                hasAllowedScope = AllowedScope.Any(providedScope.Contains);
            }
        }

        if (!hasAllowedScope)
        {
            context.Result = new ForbidResult();
        }
    }
}
