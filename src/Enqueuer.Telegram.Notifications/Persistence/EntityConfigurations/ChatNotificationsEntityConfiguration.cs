using Enqueuer.Telegram.Notifications.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Enqueuer.Telegram.Notifications.Persistence.EntityConfigurations;

internal class ChatNotificationsEntityConfiguration : IEntityTypeConfiguration<ChatNotificationsConfiguration>
{
    public void Configure(EntityTypeBuilder<ChatNotificationsConfiguration> builder)
    {
        builder.HasKey(c => c.ChatId);
        builder.Property(c => c.ChatId)
            .ValueGeneratedNever();

        builder.HasOne(c => c.NotificationsLanguage)
            .WithMany()
            .HasForeignKey(c => c.NotificationsLanguageCode);

        builder.ToTable("chat_configurations");
    }
}
