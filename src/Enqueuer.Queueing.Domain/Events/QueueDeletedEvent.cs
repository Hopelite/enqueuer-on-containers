using Enqueuer.Queueing.Domain.Models;

namespace Enqueuer.Queueing.Domain.Events;

/// <summary>
/// The domain event produced when an existing queue is deleted.
/// </summary>
public class QueueDeletedEvent : DomainEvent
{
    public QueueDeletedEvent(long groupId, string queueName, DateTime timestamp)
        : base(groupId, timestamp)
    {
        QueueName = queueName;
    }

    public override string Name => "QueueDeleted";

    /// <summary>
    /// The name of the deleted queue.
    /// </summary>
    public string QueueName { get; }

    public override void ApplyTo(Group group)
    {
        group._queues.Remove(QueueName);
    }
}