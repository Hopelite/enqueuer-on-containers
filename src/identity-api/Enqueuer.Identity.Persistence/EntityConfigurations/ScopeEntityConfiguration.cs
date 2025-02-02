using Enqueuer.Identity.Persistence.Constraints;
using Enqueuer.Identity.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Enqueuer.Identity.Persistence.EntityConfigurations;

internal class ScopeEntityConfiguration : IEntityTypeConfiguration<Scope>
{
    public void Configure(EntityTypeBuilder<Scope> builder)
    {
        builder.HasKey(s => s.Id);

        builder.HasIndex(s => s.Name)
               .IsUnique();

        builder.Property(s => s.Name)
               .HasMaxLength(ScopeConstraints.MaxScopeNameLength);

        builder.HasOne(s => s.Parent)
               .WithMany(s => s.Children)
               .HasForeignKey(s => s.ParentId)
               .OnDelete(DeleteBehavior.SetNull);
    }
}
