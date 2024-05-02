using Enqueuer.Identity.Authorization.Extensions;

namespace Enqueuer.Identity.Authorization.Models;

public class Role
{
    public Role(string name)
        : this(name, Array.Empty<Scope>())
    {
    }

    public Role(string name, IEnumerable<Scope> scopes)
    {
        Name = name.ThrowIfNullOrWhitespace(nameof(name));
        Scopes = scopes ?? throw new ArgumentNullException(nameof(scopes));
    }

    public string Name { get; }

    public IEnumerable<Scope> Scopes { get; }
}
