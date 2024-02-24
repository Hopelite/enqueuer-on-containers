using Enqueuer.Queueing.Domain.Models;

namespace Enqueuer.Queueing.Domain.Events;

/// <summary>
/// The domain event produced when new queue is created in the group.
/// </summary>
public class QueueCreatedEvent : DomainEvent
{
    public QueueCreatedEvent(long groupId, string queueName, DateTime timestamp)
        : base(groupId, timestamp)
    {
        QueueName = queueName;
    }

    public override string Name => "QueueCreated";

    /// <summary>
    /// The name of the created queue.
    /// </summary>
    public string QueueName { get; }

    public override void ApplyTo(Group group)
    {
        var createdQueue = new Queue(groupId: AggregateId, QueueName);
        group._queues.Add(createdQueue.Name, createdQueue);
    }
}
