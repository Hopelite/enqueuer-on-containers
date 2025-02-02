using Swashbuckle.AspNetCore.Annotations;
using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace Enqueuer.Identity.API.Parameters;

[SwaggerSchema(Format = "string", Description = "Space-separated list of scopes")]
public class ScopeCollection : IParsable<ScopeCollection>, IReadOnlyCollection<string>
{
    private readonly string[] _scopes;

    private ScopeCollection()
        : this(Array.Empty<string>())
    {
    }

    private ScopeCollection(string[] scopes)
    {
        _scopes = scopes;
    }

    public int Count => _scopes.Length;

    public IEnumerator<string> GetEnumerator()
        => _scopes.AsEnumerable().GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();

    /// <summary>
    /// Check whether request has the specified <paramref name="scope"/>.
    /// </summary>
    public bool ContainsScope(string scope)
        => _scopes.Contains(scope);

    public static ScopeCollection Parse(string value, IFormatProvider? provider)
    {
        if (!TryParse(value, provider, out var scopes))
        {
            throw new ArgumentNullException(nameof(value), "Scope string can't be null or empty.");
        }

        return scopes;
    }

    public static bool TryParse([NotNullWhen(true)] string? value, IFormatProvider? provider, [MaybeNullWhen(false)] out ScopeCollection result)
    {
        const string WhiteSpaceDelimiter = " ";

        var scopes = value?.Split(WhiteSpaceDelimiter, StringSplitOptions.RemoveEmptyEntries & StringSplitOptions.TrimEntries);
        if (scopes == null || scopes.Length == 0)
        {
            result = new ScopeCollection();
            return false;
        }

        result = new ScopeCollection(scopes);
        return true;
    }
}
