using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Enqueuer.Queueing.API.Authorization.Attributes;

/// <summary>
/// Specifies that the class or method that this attribute is applied to requires the authorization by scope.
/// </summary>
public class AllowedScopeAttribute : AuthorizeAttribute, IAuthorizationFilter
{
    private const string ScopeClaimType = "scope";
    private const char ScopeDelimiter = ' ';

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
            var scopeClaim = user.Claims.FirstOrDefault(c => c.Type.Equals(ScopeClaimType));
            if (scopeClaim != null)
            {
                var providedScope = scopeClaim.Value.Split(ScopeDelimiter, StringSplitOptions.RemoveEmptyEntries & StringSplitOptions.TrimEntries);
                hasAllowedScope = AllowedScope.Any(s => providedScope.Contains(s));
            }
        }

        if (!hasAllowedScope)
        {
            context.Result = new ForbidResult();
        }
    }
}
