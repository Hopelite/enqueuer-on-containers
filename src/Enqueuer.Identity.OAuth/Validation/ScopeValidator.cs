using Enqueuer.Identity.OAuth.Exceptions;
using Enqueuer.Identity.OAuth.Models;
using Enqueuer.Identity.OAuth.Storage;

namespace Enqueuer.Identity.OAuth.Validation;

public class ScopeValidator : IScopeValidator
{
    private readonly IScopeStorage _scopeStorage;

    public ScopeValidator(IScopeStorage scopeStorage)
    {
        _scopeStorage = scopeStorage;
    }

    public async ValueTask ValidateScopeAsync(Scope scope, CancellationToken cancellationToken)
    {
        if (scope == null)
        {
            throw new ArgumentNullException(nameof(scope));
        }

        foreach (var scopeValue in scope)
        {
            if (await _scopeStorage.CheckIfExistsAsync(scopeValue, cancellationToken))
            {
                throw InvalidScopeException.FromScope(scopeValue);
            }
        }
    }
}
