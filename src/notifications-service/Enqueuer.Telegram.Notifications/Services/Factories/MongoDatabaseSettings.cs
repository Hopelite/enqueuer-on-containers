namespace Enqueuer.Telegram.Notifications.Services.Factories;

public class MongoDatabaseSettings
{
    public string ConnectionString { get; init; } = null!;

    public string DatabaseName { get; init; } = null!;
}
