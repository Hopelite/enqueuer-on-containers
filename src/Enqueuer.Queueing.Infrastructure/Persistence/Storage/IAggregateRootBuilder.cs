using Enqueuer.Queueing.Domain.Events;

namespace Enqueuer.Queueing.Infrastructure.Persistence.Storage;

/// <summary>
/// Constructs <typeparamref name="TAggregate"/> from domain events.
/// </summary>
public interface IAggregateRootBuilder<TAggregate>
{
    /// <summary>
    /// Builds <typeparamref name="TAggregate"/> with the specified <paramref name="aggregateId"/> from <paramref name="domainEvents"/>.
    /// </summary>
    TAggregate Build(long aggregateId, IEnumerable<DomainEvent> domainEvents);
}
