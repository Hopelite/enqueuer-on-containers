using Enqueuer.Identity.Persistence.Models;
using Microsoft.EntityFrameworkCore;

namespace Enqueuer.Identity.Persistence;

public class IdentityContext : DbContext
{
    public IdentityContext(DbContextOptions<IdentityContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }

    public DbSet<Role> Roles { get; set; }
}
