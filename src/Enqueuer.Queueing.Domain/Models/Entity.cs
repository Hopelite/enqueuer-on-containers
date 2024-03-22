using Enqueuer.Queueing.Domain.Events;

namespace Enqueuer.Queueing.Domain.Models;

/// <summary>
/// Base class for all domain entities.
/// </summary>
public abstract class Entity
{
    private readonly List<DomainEvent> _domainEvents = new();

    /// <summary>
    /// The list of the occured domain events.
    /// </summary>
    public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    /// <summary>
    /// Adds <paramref name="event"/> to the list of the occured domain events.
    /// </summary>
    /// <remarks>Made internal to be accessed only by domain infrastructure.</remarks>
    protected void AddDomainEvent(DomainEvent @event)
    {
        _domainEvents.Add(@event);
    }
}
