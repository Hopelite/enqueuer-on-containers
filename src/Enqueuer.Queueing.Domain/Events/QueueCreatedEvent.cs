namespace Enqueuer.Queueing.Domain.Events;

public class QueueCreatedEvent: DomainEvent
{
    public QueueCreatedEvent(int queueId, string queueName, long locationId)
    {
        QueueId = queueId;
        QueueName = queueName;
        LocationId = locationId;
    }

    public int QueueId { get; }

    public string QueueName { get; }

    public long LocationId { get; }
}
