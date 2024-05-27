using Microsoft.IdentityModel.JsonWebTokens;

namespace Enqueuer.Queueing.API.Application.Claims;

public class ClaimsAccessor : IClaimsAccessor
{
    private readonly IHttpContextAccessor _contextAccessor;

    public ClaimsAccessor(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    // TODO: consider creator_id to be a string, which may be either telegram user or an app
    public long? GetUserIdFromClaims()
    {
        var httpContext = _contextAccessor.HttpContext;
        if (httpContext == null)
        {
            throw new InvalidOperationException("There is no active HttpContext being set.");
        }

        var userIdClaim = httpContext.User.FindFirst(c => c.Type == JwtRegisteredClaimNames.Sub);
        if (userIdClaim == null)
        {
            return null;
        }

        if (!long.TryParse(userIdClaim.Value, out var userId))
        {
            // TODO: consider if the exception should be thrown
            return null;
        }

        return userId;
    }
}
