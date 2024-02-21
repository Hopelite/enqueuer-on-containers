namespace Enqueuer.Queueing.Domain.Events;

/// <summary>
/// The domain event produced when participant was enqueued.
/// </summary>
public class ParticipantEnqueuedAtEvent : DomainEvent
{
    public ParticipantEnqueuedAtEvent(long groupId, string queueName, long participantId, uint position)
    {
        GroupId = groupId;
        QueueName = queueName;
        ParticipantId = participantId;
        Position = position;
    }

    public override string Name => "ParticipantEnqueuedAt";

    /// <summary>
    /// The unique identifier of the group where the queue is located.
    /// </summary>
    public long GroupId { get; }

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
}
