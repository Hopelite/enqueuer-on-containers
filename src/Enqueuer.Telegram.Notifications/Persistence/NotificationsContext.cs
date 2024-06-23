using Enqueuer.Telegram.Notifications.Persistence.Entities;
using Enqueuer.Telegram.Notifications.Persistence.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace Enqueuer.Telegram.Notifications.Persistence;

public class NotificationsContext : DbContext
{
    public DbSet<ChatNotificationsConfiguration> NotificationsConfigurations { get; set; }

    public DbSet<Language> AvailableLanguages { get; set; } // TODO: consider better way to store them. Maybe retrieve from the Resources

    public NotificationsContext(DbContextOptions<NotificationsContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .ApplyConfiguration(new ChatNotificationsEntityConfiguration())
            .ApplyConfiguration(new LanguageCodeEntityConfiguration());
    }
}
