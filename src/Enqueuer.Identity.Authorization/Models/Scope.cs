using Enqueuer.Identity.Authorization.Extensions;

namespace Enqueuer.Identity.Authorization.Models;

public class Scope
{
    public Scope(string name)
    {
        Name = name.ThrowIfNullOrWhitespace(nameof(name));
    }

    public string Name { get; }
}
