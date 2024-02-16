using Enqueuer.Queueing.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Enqueuer.Queueing.API.Extensions;

public static class DatabaseMigration
{
    public static WebApplication MigrateDatabase(this WebApplication app)
    {
        var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
        using (var scope = scopeFactory.CreateScope())
        {
            var queueingContext = scope.ServiceProvider.GetRequiredService<QueueingContext>();
            queueingContext.Database.Migrate();
        }

        return app;
    }
}
