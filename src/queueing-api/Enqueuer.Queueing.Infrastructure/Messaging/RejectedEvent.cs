using Enqueuer.Queueing.Domain.Events;
using Enqueuer.Queueing.Domain.Models;

namespace Enqueuer.Queueing.Infrastructure.Messaging;

public class RejectedEvent : DomainEvent
{
    public RejectedEvent(DomainEvent domainEvent, Exception exception)
        : base(domainEvent.AggregateId, domainEvent.Timestamp)
    {
        DomainEvent = domainEvent;
        Exception = exception;
    }

    public override string Name => $"Rejected{DomainEvent.Name}";

    public DomainEvent DomainEvent { get; }

    public Exception Exception { get; }

    public override void ApplyTo(Group group)
    {
        DomainEvent.ApplyTo(group);
    }
}
