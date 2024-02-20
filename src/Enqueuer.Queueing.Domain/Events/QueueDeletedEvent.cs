namespace Enqueuer.Queueing.Domain.Events;

/// <summary>
/// The domain event produced when an existing queue is deleted.
/// </summary>
public class QueueDeletedEvent : DomainEvent
{
    public QueueDeletedEvent(long groupId, string queueName)
    {
        GroupId = groupId;
        QueueName = queueName;
    }

    public override string Name => "QueueDeleted";

    /// <summary>
    /// The unique identifier of the group where the deleted queue was related.
    /// </summary>
    public long GroupId { get; }

    /// <summary>
    /// The name of the deleted queue.
    /// </summary>
    public string QueueName { get; }
}