using Enqueuer.Queueing.Domain.Exceptions;
using Enqueuer.Queueing.Domain.Models;

namespace Enqueuer.Queueing.Domain.Events;

/// <summary>
/// The domain event produced when participant was enqueued.
/// </summary>
public class ParticipantEnqueuedEvent : DomainEvent
{
    public ParticipantEnqueuedEvent(long groupId, string queueName, long participantId, DateTime timestamp)
        : base(groupId, timestamp)
    {
        QueueName = queueName;
        ParticipantId = participantId;
    }

    public override string Name => "ParticipantEnqueued";

    /// <summary>
    /// The name of the queue where participant was enqueued.
    /// </summary>
    public string QueueName { get; }

    /// <summary>
    /// The unique identifier of the enqueued participant.
    /// </summary>
    public long ParticipantId { get; }

    public override void ApplyTo(Group group)
    {
        if (!group._queues.TryGetValue(QueueName, out var queue))
        {
            throw new QueueDoesNotExistException($"Queue '{QueueName}' does not exist in the group '{Id}'.");
        }

        (queue as IQueueEntity).EnqueueParticipant(ParticipantId);
    }
}
