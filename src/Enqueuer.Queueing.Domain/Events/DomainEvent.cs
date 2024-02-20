namespace Enqueuer.Queueing.Domain.Events;

/// <summary>
/// The base class for all domain events.
/// </summary>
public abstract class DomainEvent
{
    /// <summary>
    /// The name of the domain event.
    /// </summary>
    public abstract string Name { get; }

    /// <summary>
    /// The date and time when this domain event was produced.
    /// </summary>
    public DateTime Timestamp => DateTime.UtcNow; // TODO: not sure about both usage of timestamps and readonly getters here
}
