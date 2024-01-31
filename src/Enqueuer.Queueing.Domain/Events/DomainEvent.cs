namespace Enqueuer.Queueing.Domain.Events;

public abstract class DomainEvent
{
    public abstract string Name { get; }
}
