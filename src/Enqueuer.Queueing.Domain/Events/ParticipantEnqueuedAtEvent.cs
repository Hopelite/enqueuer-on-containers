using Enqueuer.Queueing.Domain.Exceptions;
using Enqueuer.Queueing.Domain.Models;

namespace Enqueuer.Queueing.Domain.Events;

/// <summary>
/// The domain event produced when participant was enqueued.
/// </summary>
public class ParticipantEnqueuedAtEvent : DomainEvent
{
    public ParticipantEnqueuedAtEvent(long groupId, string queueName, long participantId, uint position, DateTime timestamp)
        : base(groupId, timestamp)
    {
        QueueName = queueName;
        ParticipantId = participantId;
        Position = position;
    }

    public override string Name => "ParticipantEnqueuedAt";

    /// <summary>
    /// The name of the queue where participant was enqueued.
    /// </summary>
    public string QueueName { get; }

    /// <summary>
    /// The unique identifier of the enqueued participant.
    /// </summary>
    public long ParticipantId { get; }

    /// <summary>
    /// The position participant was enqueued at.
    /// </summary>
    public uint Position { get; }

    public override void ApplyTo(Group group)
    {
        if (!group._queues.TryGetValue(QueueName, out var queue))
        {
            throw new QueueDoesNotExistException(QueueName, $"Queue '{QueueName}' does not exist in the group '{Id}'.");
        }

        (queue as IQueueEntity).EnqueueParticipantAt(ParticipantId, Position);
    }
}
