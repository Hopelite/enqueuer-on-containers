using Enqueuer.Identity.Authorization.Models;

namespace Enqueuer.Identity.Authorization.Scopes;

public interface IScopeValidator
{
    void Validate(Scope scope);
}
