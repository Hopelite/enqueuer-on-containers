namespace Enqueuer.Queueing.Domain.Events;

public class QueueRemovedEvent : DomainEvent
{
    public QueueRemovedEvent(long queueId, string queueName, long locationId)
    {
        QueueId = queueId;
        QueueName = queueName;
        LocationId = locationId;
    }

    public override string Name => nameof(QueueCreatedEvent);

    public long QueueId { get; }

    public string QueueName { get; }

    public long LocationId { get; }
}
