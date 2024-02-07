using Enqueuer.Telegram.Notifications.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Enqueuer.Telegram.Notifications.Persistence.EntityConfigurations;

public class LanguageCodeEntityConfiguration : IEntityTypeConfiguration<Language>
{
    public void Configure(EntityTypeBuilder<Language> builder)
    {
        builder.HasKey(c => c.Code);

        builder.HasMany(c => c.LocalizedMessages)
            .WithOne()
            .HasForeignKey(m => m.LanguageCode);

        builder.ToTable("languages");
    }
}
