namespace Enqueuer.Identity.Authorization.Grants;

public class AuthorizationContext : IAuthorizationContext
{
    public AuthorizationContext(IServiceProvider serviceProvider)
    {
        Services = serviceProvider;
    }

    public IServiceProvider Services { get; }
}
