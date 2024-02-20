namespace Enqueuer.Queueing.Domain.Events;

/// <summary>
/// The domain event produced when new queue is created in the group.
/// </summary>
public class QueueCreatedEvent : DomainEvent
{
    public QueueCreatedEvent(long groupId, string queueName)
    {
        GroupId = groupId;
        QueueName = queueName;
    }

    public override string Name => "QueueCreated";

    /// <summary>
    /// The unique identifier of the group where the queue has been created.
    /// </summary>
    public long GroupId { get; }

    /// <summary>
    /// The name of the created queue.
    /// </summary>
    public string QueueName { get; }
}
