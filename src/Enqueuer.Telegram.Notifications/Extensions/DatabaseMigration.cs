using Enqueuer.Telegram.Notifications.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Microsoft.Extensions.DependencyInjection;

public static class DatabaseMigration
{
    public static IServiceCollection MigrateDatabase(this IServiceCollection services)
    {
        services.AddHostedService<MigrationRunner>();
        return services;
    }

    private class MigrationRunner : IHostedService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<MigrationRunner> _logger;

        public MigrationRunner(IServiceScopeFactory scopeFactory, ILogger<MigrationRunner> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var queueingContext = scope.ServiceProvider.GetRequiredService<NotificationsContext>();

                await queueingContext.Database.MigrateAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An exception was thrown during database migration.");
                throw;
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
