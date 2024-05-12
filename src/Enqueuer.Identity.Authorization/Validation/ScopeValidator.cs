using Enqueuer.Identity.Authorization.Models;
using Enqueuer.Identity.Authorization.Validation.Exceptions;
using Enqueuer.Identity.Persistence.Constraints;
using System.Text.RegularExpressions;

namespace Enqueuer.Identity.Authorization.Validation;

public class ScopeValidator : IScopeValidator
{
    private static readonly Regex ScopeNameRegex = new Regex(@"^[a-z0-9:]+$", RegexOptions.Compiled);

    public void Validate(Scope scope)
    {
        ValidateScope(scope);
    }

    private static void ValidateScope(Scope scope, int currentDepth = 1)
    {
        if (scope == null)
        {
            throw new ValidationException("An exception was thrown during scope validation.", new ArgumentNullException(nameof(scope)));
        }

        if (scope.Name.Length > ScopeConstraints.MaxScopeNameLength || scope.Name.Length < ScopeConstraints.MinScopeNameLength)
        {
            throw new InvalidScopeNameException($"The legnth of the scope name must be from {ScopeConstraints.MinScopeNameLength} to {ScopeConstraints.MaxScopeNameLength} characters.");
        }

        if (!ScopeNameRegex.IsMatch(scope.Name))
        {
            throw new InvalidScopeNameException("The scope name may contain only lower-case letters, digits and colon.");
        }

        CheckNestingDepth(scope, currentDepth);
    }

    private static void CheckNestingDepth(Scope scope, int currentDepth)
    {
        if (currentDepth > ScopeConstraints.MaxNestingDepth)
        {
            throw new NestingIsTooDeepException($"The maximum depth of the scope nesting allowed, including the parent scope, is {ScopeConstraints.MaxNestingDepth}.");
        }

        if (scope.ChildScopes != null && scope.ChildScopes.Any())
        {
            foreach (var childScope in scope.ChildScopes)
            {
                ValidateScope(childScope, currentDepth + 1);
            }
        }
    }
}
