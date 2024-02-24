using Enqueuer.Queueing.Domain.Events;

namespace Enqueuer.Queueing.Infrastructure.Persistence.Storage;

public interface IEventStorage
{
    /// <summary>
    /// Gets all domain events associated with the aggregate with the specified <paramref name="aggregateId"/>.
    /// </summary>
    Task<IEnumerable<DomainEvent>> GetAggregateEventsAsync(long aggregateId, CancellationToken cancellationToken);

    /// <summary>
    /// Writes <paramref name="events"/> into the events storage.
    /// </summary>
    Task WriteEventsAsync(IEnumerable<DomainEvent> events, CancellationToken cancellationToken);
}
