using Enqueuer.Identity.Authorization.Models;
using Enqueuer.Identity.Persistence.Constraints;

namespace Enqueuer.Identity.Authorization.Scopes;

public class ScopeValidator : IScopeValidator
{
    public void Validate(Scope scope)
    {
        if (scope == null)
        {
            throw new ArgumentNullException(nameof(scope));
        }

        if (scope.Name.Length > ScopeConstraints.MaxScopeNameLength || scope.Name.Length < ScopeConstraints.MinScopeNameLength)
        {
            throw new InvalidScopeNameException($"The legnth of the scope name must be from {ScopeConstraints.MinScopeNameLength} to {ScopeConstraints.MaxScopeNameLength} characters.");
        }

        if (!CheckNestingDepth(scope))
        {
            throw new NestingIsTooDeepException($"The maximum depth of the scope nesting allowed, including the parent scope, is {ScopeConstraints.MaxNestingDepth}.");
        }
    }

    private static bool CheckNestingDepth(Scope scope)
    {
        return CheckNestingDepthCore(scope, currentDepth: 1);
    }

    private static bool CheckNestingDepthCore(Scope scope, int currentDepth)
    {
        if (currentDepth >= ScopeConstraints.MaxNestingDepth)
        {
            return false;
        }

        if (scope.ChildScopes != null && scope.ChildScopes.Any())
        {
            foreach (var childScope in scope.ChildScopes)
            {
                if (!CheckNestingDepthCore(childScope, currentDepth + 1))
                {
                    return false;
                }
            }
        }

        return true;
    }
}
