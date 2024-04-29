using Enqueuer.Identity.Authorization.Extensions;

namespace Enqueuer.Identity.Authorization.Models;

public class Role
{
    public Role(string name)
    {
        Name = name.ThrowIfNullOrWhitespace(nameof(name));
    }

    public string Name { get; }
}
