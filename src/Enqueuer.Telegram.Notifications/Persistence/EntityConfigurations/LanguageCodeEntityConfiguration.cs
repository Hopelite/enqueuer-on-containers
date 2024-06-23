using Enqueuer.Telegram.Notifications.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Enqueuer.Telegram.Notifications.Persistence.EntityConfigurations;

public class LanguageCodeEntityConfiguration : IEntityTypeConfiguration<Language>
{
    public void Configure(EntityTypeBuilder<Language> builder)
    {
        builder.HasKey(c => c.Code);

        builder.ToTable("languages");
    }
}
