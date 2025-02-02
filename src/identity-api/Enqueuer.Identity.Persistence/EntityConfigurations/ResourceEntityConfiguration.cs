using Enqueuer.Identity.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Enqueuer.Identity.Persistence.EntityConfigurations;

internal class ResourceEntityConfiguration : IEntityTypeConfiguration<Resource>
{
    private const int MaxUriLength = 256;

    public void Configure(EntityTypeBuilder<Resource> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Uri)
            .HasConversion<string>()
            .HasMaxLength(MaxUriLength);

        builder.HasOne(r => r.Parent)
               .WithMany(r => r.Children)
               .HasForeignKey(r => r.ParentId)
               .OnDelete(DeleteBehavior.SetNull);
    }
}
