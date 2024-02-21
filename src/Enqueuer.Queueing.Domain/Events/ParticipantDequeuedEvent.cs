namespace Enqueuer.Queueing.Domain.Events;

/// <summary>
/// The domain event produced when participant was dequeued.
/// </summary>
public class ParticipantDequeuedEvent : DomainEvent
{
    public ParticipantDequeuedEvent(long groupId, string queueName, long participantId)
    {
        GroupId = groupId;
        QueueName = queueName;
        ParticipantId = participantId;
    }

    public override string Name => "ParticipantDequeued";

    /// <summary>
    /// The unique identifier of the group where the queue is located.
    /// </summary>
    public long GroupId { get; }

    /// <summary>
    /// The name of the queue from which participant was dequeued.
    /// </summary>
    public string QueueName { get; }

    /// <summary>
    /// The unique identifier of the dequeued participant.
    /// </summary>
    public long ParticipantId { get; }
}
