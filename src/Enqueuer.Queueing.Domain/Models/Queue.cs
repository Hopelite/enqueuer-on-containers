using Enqueuer.Queueing.Domain.Events;
using Enqueuer.Queueing.Domain.Exceptions;
using System.Diagnostics.CodeAnalysis;

namespace Enqueuer.Queueing.Domain.Models;

/// <summary>
/// The domain model representing an ordered sequence of participants.
/// </summary>
public class Queue : Entity, IQueueEntity
{
    private static readonly IdentityComparer ParticipantIdentityComparer = new();
    internal readonly Dictionary<uint, Participant> _participants = new();

    internal Queue(long groupId, string name)
    {
        GroupId = groupId;
        Name = name;
    }

    /// <summary>
    /// The unique identifier of the group this queue is related.
    /// </summary>
    public long GroupId { get; }

    /// <summary>
    /// The name of the queue. Must be unique within the group scope.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// The ordered sequence of reserved positions.
    /// </summary>
    public IReadOnlyCollection<Participant> Participants => _participants.Values;

    /// <summary>
    /// Adds the participant with the specified <paramref name="participantId"/>
    /// at the first available position in the queue.
    /// </summary>
    /// <exception cref="ParticipantAlreadyExistsException">Thrown, if the participant with the specified <paramref name="participantId"/> already exists in the queue.</exception>
    internal void EnqueueParticipant(long participantId)
    {
        (this as IQueueEntity).EnqueueParticipant(participantId);
        AddDomainEvent(new ParticipantEnqueuedEvent(GroupId, queueName: Name, participantId, DateTime.UtcNow));
    }

    void IQueueEntity.EnqueueParticipant(long participantId)
    {
        var firstAvailablePosition = Position.GetFirstAvailablePosition(_participants.Keys);

        var participant = new Participant(participantId, firstAvailablePosition);
        if (_participants.Values.Contains(participant, ParticipantIdentityComparer))
        {
            throw new ParticipantAlreadyExistsException(
                $"Participant '{participant.Id}' already exists in the queue '{Name}'.");
        }

        if (!_participants.TryAdd(firstAvailablePosition.Number, participant))
        {
            throw new PositionReservedException(
                $"Cannot enqueue participant '{participant.Id}' to the reserved position '{participant.Position}' in the queue '{Name}'.");
        }
    }

    /// <summary>
    /// Adds the participant with the specified <paramref name="participantId"/>
    /// at the specified <paramref name="position"/> in queue.
    /// </summary>
    /// <param name="participantId">The unique identifier of the participant placed in the queue.</param>
    /// <remarks>
    /// Implemented as a separate method instead of making the <see cref="Participants"/>
    /// of <see cref="ICollection{T}"/> type to enforce the CQRS pattern.
    /// </remarks>
    /// <exception cref="PositionReservedException">Thrown, if participant's position is reserved.</exception>
    /// <exception cref="ParticipantAlreadyExistsException">Thrown, if the participant with the specified <paramref name="participantId"/> already exists in the queue.</exception>
    internal void EnqueueParticipantAt(long participantId, uint position)
    {
        (this as IQueueEntity).EnqueueParticipantAt(participantId, position);
        AddDomainEvent(new ParticipantEnqueuedAtEvent(GroupId, queueName: Name, participantId, position, DateTime.UtcNow));
    }

    void IQueueEntity.EnqueueParticipantAt(long participantId, uint position)
    {
        // TODO: add position validation

        var participant = new Participant(participantId, position);
        if (_participants.Values.Contains(participant, ParticipantIdentityComparer))
        {
            throw new ParticipantAlreadyExistsException(
                $"Participant '{participant.Id}' already exists in the queue '{Name}'.");
        }

        if (!_participants.TryAdd(position, participant))
        {
            throw new PositionReservedException(
                $"Cannot enqueue participant '{participant.Id}' to the reserved position '{participant.Position}' in the queue '{Name}'.");
        }
    }

    /// <summary>
    /// Dequeues a participant with the specified <paramref name="participantId"/> from the queue.
    /// </summary>
    /// <param name="participantId">The unique identifier of the participant removed from the queue.</param>
    /// <remarks>
    internal void DequeueParticipant(long participantId)
    {
        (this as IQueueEntity).DequeueParticipant(participantId);
        AddDomainEvent(new ParticipantDequeuedEvent(GroupId, queueName: Name, participantId, DateTime.UtcNow));
    }

    void IQueueEntity.DequeueParticipant(long participantId)
    {
        var participant = _participants.Values.FirstOrDefault(p => p.Id == participantId);
        if (participant == null || !_participants.Remove(participant.Position.Number))
        {
            throw new ParticipantDoesNotExistException(
                $"Participant '{participantId}' does not exist in the queue '{Name}'.");
        }
    }

    private class IdentityComparer : IEqualityComparer<Participant>
    {
        public bool Equals(Participant? first, Participant? second)
        {
            return (first, second) switch
            {
                (null, null) => true,
                (null, _) => false,
                (_, null) => false,
                _ => first.Id == second.Id,
            };
        }

        public int GetHashCode([DisallowNull] Participant participant)
        {
            return participant.Id.GetHashCode();
        }
    }
}
