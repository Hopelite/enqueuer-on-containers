using Enqueuer.Queueing.Domain.Exceptions;
using Enqueuer.Queueing.Domain.Models;

namespace Enqueuer.Queueing.Domain.Events;

/// <summary>
/// The domain event produced when new queue is created in the group.
/// </summary>
public class QueueCreatedEvent : DomainEvent
{
    public QueueCreatedEvent(long groupId, string queueName, long creatorId, DateTime timestamp)
        : base(groupId, timestamp)
    {
        QueueName = queueName;
        CreatorId = creatorId;
    }

    public override string Name => "QueueCreated";

    /// <summary>
    /// The name of the created queue.
    /// </summary>
    public string QueueName { get; }

    /// <summary>
    /// The unique identifier of the queue creator.
    /// </summary>
    public long CreatorId { get; }

    public override void ApplyTo(Group group)
    {
        var createdQueue = new Queue(groupId: AggregateId, QueueName);
        if (!group._queues.TryAdd(createdQueue.Name, createdQueue))
        {
            throw new QueueAlreadyExistsException(QueueName, $"Queue '{QueueName}' already exists in the group '{AggregateId}'.");
        }
    }
}
