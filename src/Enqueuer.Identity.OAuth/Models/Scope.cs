using System.Collections;

namespace Enqueuer.Identity.OAuth.Models;

/// <summary>
/// Represents an authorization scope.
/// </summary>
public class Scope : IReadOnlyCollection<string>
{
    private readonly HashSet<string> _values;

    public Scope(string? value)
        : this(GetScopeValues(value))
    {
    }

    public Scope(IReadOnlyCollection<string> values)
    {
        if (_values == null)
        {
            _values = new HashSet<string>(capacity: 0);
            return;
        }    

        _values = new HashSet<string>(values);
    }

    public int Count => _values.Count;

    /// <summary>
    /// Checks whether the scope parameter contains any value.
    /// </summary>
    public bool HasValue => _values.Count > 0;

    public IEnumerator<string> GetEnumerator()
    {
        return _values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private static string[] GetScopeValues(string? scope)
    {
        const char ScopeDelimiter = ' ';
        if (scope == null)
        {
            return Array.Empty<string>();
        }

        return scope.Split(ScopeDelimiter, StringSplitOptions.RemoveEmptyEntries);
    }
}
