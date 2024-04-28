namespace Enqueuer.Identity.API.Services.Scopes;

public interface IScopeValidator
{
    bool Validate(string scope);
}
