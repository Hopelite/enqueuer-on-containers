using Enqueuer.Telegram.Notifications.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Enqueuer.Telegram.Notifications.Persistence.EntityConfigurations;

public class NotificationMessageEntityConfiguration : IEntityTypeConfiguration<NotificationMessage>
{
    public void Configure(EntityTypeBuilder<NotificationMessage> builder)
    {
        builder.HasKey(m => new { m.LanguageCode, m.Key });

        builder.ToTable("localized_messages");
    }
}
