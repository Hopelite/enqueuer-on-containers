namespace Enqueuer.Queueing.API.Application.Claims;

public interface IClaimsAccessor
{
    long? GetUserIdFromClaims();
}
