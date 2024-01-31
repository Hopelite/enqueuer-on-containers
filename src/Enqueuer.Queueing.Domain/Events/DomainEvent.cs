namespace Enqueuer.Queueing.Domain.Events;

public abstract class DomainEvent
{
    public DomainEvent()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
    }

    public Guid Id { get; }

    public DateTime CreatedAt { get; }
}
