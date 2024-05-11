using Enqueuer.Identity.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Enqueuer.Identity.Persistence.EntityConfigurations;

internal class UserEntityConfiguration : IEntityTypeConfiguration<User>
{
    private const int TelegramMaxNameLength = 64;

    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);

        builder.HasIndex(u => u.UserId)
               .IsUnique();

        builder.Property(u => u.FirstName)
               .HasMaxLength(TelegramMaxNameLength);

        builder.Property(u => u.LastName)
               .HasMaxLength(TelegramMaxNameLength);
    }
}
