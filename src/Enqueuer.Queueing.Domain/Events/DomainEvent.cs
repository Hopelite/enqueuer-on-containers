using Enqueuer.Queueing.Domain.Models;

namespace Enqueuer.Queueing.Domain.Events;

/// <summary>
/// The base class for all domain events.
/// </summary>
public abstract class DomainEvent
{
    public string Id { get; set; }

    protected DomainEvent(long aggregateId, DateTime timestamp)
    {
        AggregateId = aggregateId;
        Timestamp = timestamp;
    }

    /// <summary>
    /// The unique identifier of an aggregate model this event is related to.
    /// </summary>
    public long AggregateId { get; }

    /// <summary>
    /// The date and time when this event was produced.
    /// </summary>
    public DateTime Timestamp { get; }

    /// <summary>
    /// The name of the domain event.
    /// </summary>
    public abstract string Name { get; }

    /// <summary>
    /// Applies the event to the root aggregate which is <see cref="Group"/>.
    /// </summary>
    public abstract void ApplyTo(Group group);
}
