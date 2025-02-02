namespace Enqueuer.Identity.Persistence.Constraints;

public static class ScopeConstraints
{
    /// <summary>
    /// The maximal length of the scope name.
    /// </summary>
    public const int MaxScopeNameLength = 64;

    /// <summary>
    /// The minimal length of the scope name.
    /// </summary>
    public const int MinScopeNameLength = 2;

    /// <summary>
    /// The maximum depth of the single scope inheritance. This value includes the parent scope in count.
    /// </summary>
    public const int MaxNestingDepth = 3;
}
