namespace Enqueuer.Identity.Authorization.Grants;

public interface IAuthorizationContext
{
    public IServiceProvider Services { get; }
}
