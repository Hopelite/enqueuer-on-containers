using Enqueuer.OAuth.Core.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Microsoft.AspNetCore.Authorization;

/// <summary>
/// Specifies that the class or method that this attribute is applied to requires the authorization by scope.
/// </summary>
public class AllowedScopesAttribute : AuthorizeAttribute, IAuthorizationFilter
{
    public AllowedScopesAttribute()
        : this(Array.Empty<string>())
    {
    }

    public AllowedScopesAttribute(params string[] allowedScope)
    {
        AllowedScopes = allowedScope;
    }

    /// <summary>
    /// Gets or sets a list of allowed scopes.
    /// </summary>
    public string[] AllowedScopes { get; init; }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var hasAllowedScope = false;

        var user = context.HttpContext.User;
        if (user.Identity != null && user.Identity.IsAuthenticated)
        {
            var scopeClaim = user.Claims.FirstOrDefault(c => c.Type.Equals(ScopeClaim.ClaimType));
            if (scopeClaim != null)
            {
                var providedScope = ScopeClaim.Create(scopeClaim.Value);
                hasAllowedScope = AllowedScopes.Any(providedScope.Contains);
            }
        }

        if (!hasAllowedScope)
        {
            context.Result = new ForbidResult();
        }
    }
}
