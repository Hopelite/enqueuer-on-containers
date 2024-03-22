using Enqueuer.Queueing.Domain.Events;

namespace Enqueuer.Queueing.Infrastructure.Persistence.Storage;

/// <summary>
/// Provides functionality to read domain events from and write to storage.
/// </summary>
public interface IEventStorage
{
    /// <summary>
    /// Gets all domain events associated with the aggregate with the specified <paramref name="aggregateId"/>.
    /// </summary>
    Task<IEnumerable<DomainEvent>> GetAggregateEventsAsync(long aggregateId, CancellationToken cancellationToken);

    /// <summary>
    /// Writes <paramref name="event"/> into the events storage.
    /// </summary>
    Task WriteEventAsync(DomainEvent @event, CancellationToken cancellationToken);
}
