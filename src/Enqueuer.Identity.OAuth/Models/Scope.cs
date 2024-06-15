using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace Enqueuer.Identity.OAuth.Models;

/// <summary>
/// Represents an authorization scope.
/// </summary>
public class Scope : IReadOnlyCollection<string>
{
    private const char ScopeDelimiter = ' ';
    private readonly string? _value;
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
        _value = GetScopeValue(_values);
    }

    /// <summary>
    /// The space-delimitered value of the scope.
    /// </summary>
    public string? Value => _value;

    public int Count => _values.Count;

    /// <summary>
    /// Checks whether the scope parameter contains any value.
    /// </summary>
    [MemberNotNullWhen(returnValue: true, nameof(Value))]
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

    private static string? GetScopeValue(IReadOnlyCollection<string> values)
    {
        if (values == null || values.Count == 0)
        {
            return null;
        }

        return string.Join(ScopeDelimiter, values);
    }
}