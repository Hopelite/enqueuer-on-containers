using Enqueuer.Identity.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Enqueuer.Identity.Persistence.EntityConfigurations;

internal class RoleEntityConfiguration : IEntityTypeConfiguration<Role>
{
    private const int MaxRoleNameLength = 64;

    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasKey(r => r.Id);

        builder.HasIndex(r => r.Name)
               .IsUnique();

        builder.Property(r => r.Name)
               .HasMaxLength(MaxRoleNameLength);

        builder.HasMany(r => r.Scopes)
               .WithMany(s => s.Roles);
    }
}
