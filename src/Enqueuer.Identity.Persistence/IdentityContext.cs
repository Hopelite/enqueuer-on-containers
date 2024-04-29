using Enqueuer.Identity.Persistence.EntityConfigurations;
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

    public DbSet<Scope> Scopes { get; set; }

    public DbSet<Resource> Resources { get; set; }

    public DbSet<UserResourceRoles> UserResourceRoles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserEntityConfiguration())
                    .ApplyConfiguration(new RoleEntityConfiguration())
                    .ApplyConfiguration(new ScopeEntityConfiguration())
                    .ApplyConfiguration(new ResourceEntityConfiguration())
                    .ApplyConfiguration(new UserResourceRolesEntityConfiguration());
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSnakeCaseNamingConvention();
    }
}
