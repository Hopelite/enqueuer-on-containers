using Enqueuer.Identity.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Enqueuer.Identity.Persistence.EntityConfigurations;

internal class UserResourceRolesEntityConfiguration : IEntityTypeConfiguration<UserResourceRoles>
{
    public void Configure(EntityTypeBuilder<UserResourceRoles> builder)
    {
        builder.HasKey(r => new { r.UserId, r.ResourceId });

        builder.HasOne(r => r.User)
               .WithMany(u => u.ResourceRoles)
               .HasForeignKey(r => r.UserId);

        builder.HasOne(r => r.Resource)
               .WithMany(r => r.ResourceRoles)
               .HasForeignKey(r => r.ResourceId);

        builder.HasOne(r => r.Role)
               .WithMany(r => r.ResourceRoles)
               .HasForeignKey(r => r.RoleId);
    }
}
