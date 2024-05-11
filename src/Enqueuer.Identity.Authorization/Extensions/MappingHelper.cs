namespace Enqueuer.Identity.Authorization.Extensions;

public static class MappingHelper
{
    public static TResult MapRecursive<TSource, TResult>(
        this TSource source,
        Func<TSource, IEnumerable<TResult>, TResult> mapper,
        Func<TSource, IEnumerable<TSource>?> childAccessor,
        int maxRecursionDepth = 5)
    {
        return MapRecursiveCore(source, mapper, childAccessor, maxRecursionDepth);
    }

    private static TResult MapRecursiveCore<TSource, TResult>(
        TSource source,
        Func<TSource, IEnumerable<TResult>, TResult> mapper,
        Func<TSource, IEnumerable<TSource>?> childAccessor,
        int maxRecursionDepth,
        int currentDepth = 0)
    {
        if (currentDepth >= maxRecursionDepth)
        {
            throw new InvalidOperationException("");
        }

        var mappedChildren = new List<TResult>();
        var childInstances = childAccessor(source);

        if (childInstances != null && childInstances.Any())
        {
            foreach (var childScope in childInstances)
            {
                mappedChildren.Add(MapRecursiveCore(childScope, mapper, childAccessor, maxRecursionDepth, currentDepth + 1));
            }
        }

        return mapper(source, mappedChildren);
    }
}
