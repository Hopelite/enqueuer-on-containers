using Enqueuer.Queueing.Domain.Exceptions;
using Enqueuer.Queueing.Domain.Models;

namespace Enqueuer.Queueing.Domain.Events;

/// <summary>
/// The domain event produced when participant was dequeued.
/// </summary>
public class ParticipantDequeuedEvent : DomainEvent
{
    public ParticipantDequeuedEvent(long groupId, string queueName, long participantId, DateTime timestamp)
        : base(groupId, timestamp)
    {
        QueueName = queueName;
        ParticipantId = participantId;
    }

    public override string Name => "ParticipantDequeued";

    /// <summary>
    /// The name of the queue from which participant was dequeued.
    /// </summary>
    public string QueueName { get; }

    /// <summary>
    /// The unique identifier of the dequeued participant.
    /// </summary>
    public long ParticipantId { get; }

    public override void ApplyTo(Group group)
    {
        if (!group._queues.TryGetValue(QueueName, out var queue))
        {
            throw new QueueDoesNotExistException($"Queue '{QueueName}' does not exist.");
        }


    }
}
