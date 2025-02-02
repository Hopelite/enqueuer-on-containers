using Enqueuer.Identity.Authorization.Models;

namespace Enqueuer.Identity.Authorization.Validation;

public interface IScopeValidator
{
    /// <summary>
    /// Checks if the <paramref name="scope"/> is valid.
    /// </summary>
    void Validate(Scope scope);
}
