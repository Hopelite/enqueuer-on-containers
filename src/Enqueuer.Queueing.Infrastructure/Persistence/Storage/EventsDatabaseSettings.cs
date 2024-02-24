namespace Enqueuer.Queueing.Infrastructure.Persistence.Storage;

public class EventsDatabaseSettings
{
    public string ConnectionString { get; init; } = null!;

    public string DatabaseName { get; init; } = null!;

    public string EventsCollectionName { get; init; } = null!;
}
