using System;
using System.Collections;
using System.Collections.Generic;

namespace Enqueuer.OAuth.Core.Models
{
    /// <summary>
    /// Represents an OAuth scope.
    /// </summary>
    public class Scope : IReadOnlyCollection<string>
    {
        private const char ScopeDelimiter = ' ';
        private readonly string? _value;
        private readonly HashSet<string> _values;

        public static Scope Empty => new Scope(values: Array.Empty<string>());

        public Scope(string? value)
            : this(GetScopeValues(value))
        {
        }

        public Scope(IReadOnlyCollection<string> values)
        {
            if (values == null || values.Count == 0)
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
        public bool HasValue => _values.Count > 0;

        /// <summary>
        /// Determines whether this scope has the specified value.
        /// </summary>
        public bool Contains(string value)
        {
            return _values.Contains(value);
        }

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
}
