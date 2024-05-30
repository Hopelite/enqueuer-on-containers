using Enqueuer.Telegram.Notifications.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Microsoft.Extensions.DependencyInjection;

public static class DatabaseMigrationExtensions
{
    public static WebApplication MigrateDatabase(this WebApplication app)
    {
        var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
        using (var scope = scopeFactory.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<NotificationsContext>();
            dbContext.Database.Migrate();
        }

        return app;
    }
}
