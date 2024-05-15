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
        public const char ScopeDelimiter = ' ';
        private readonly HashSet<string> _values;

        private Scope(string value, IReadOnlyCollection<string> values)
        {
            Value = value;
            _values = new HashSet<string>(values);
        }

        /// <summary>
        /// The space-delimited values of the scope.
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// The list of values set for this scope.
        /// </summary>
        public IReadOnlyCollection<string> Values => _values;

        public int Count => Values.Count;

        /// <summary>
        /// Determines whether this scope has the specified value.
        /// </summary>
        public bool Contains(string value)
        {
            return _values.Contains(value);
        }

        public IEnumerator<string> GetEnumerator()
        {
            return Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Creates a <see cref="Scope"/> containing the specified <paramref name="values"/>.
        /// </summary>
        public static Scope Create(IReadOnlyCollection<string> values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            if (values.Count == 0)
            {
                throw new ArgumentException("Scope value can't be empty.", nameof(values));
            }

            var scopeValue = string.Join(ScopeDelimiter, values);
            return new Scope(scopeValue, values);
        }

        /// <summary>
        /// Creates a single <see cref="Scope"/> containing the specified values of the <paramref name="value"/> string.
        /// </summary>
        public static Scope Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            var values = value.Split(ScopeDelimiter, StringSplitOptions.RemoveEmptyEntries);
            return new Scope(value, values);
        }
    }
}
