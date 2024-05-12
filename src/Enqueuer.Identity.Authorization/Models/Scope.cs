using Enqueuer.Identity.Authorization.Extensions;
using System.Text.Json.Serialization;

namespace Enqueuer.Identity.Authorization.Models;

public class Scope : IEquatable<Scope>
{
    public Scope(string name)
        : this(name, Array.Empty<Scope>())
    {
    }

    [JsonConstructor]
    public Scope(string name, IEnumerable<Scope>? childScopes)
    {
        Name = name.ThrowIfNullOrWhitespace(nameof(name));
        ChildScopes = childScopes;
    }

    public string Name { get; }

    public IEnumerable<Scope>? ChildScopes { get; }

    public override string ToString()
    {
        return Name;
    }

    public bool Equals(Scope? other)
    {
        return other != null && Name.Equals(other.Name);
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as Scope);
    }

    public override int GetHashCode()
    {
        return Name.GetHashCode();
    }
}
