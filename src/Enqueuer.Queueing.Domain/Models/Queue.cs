using Enqueuer.Queueing.Domain.Events;
using Enqueuer.Queueing.Domain.Exceptions;
using System.Diagnostics.CodeAnalysis;

namespace Enqueuer.Queueing.Domain.Models;

/// <summary>
/// The domain model representing an ordered sequence of participants.
/// </summary>
public class Queue : Entity
{
    private static readonly IdentityComparer ParticipantIdentityComparer = new();
    private readonly Dictionary<uint, Participant> _participants = new();

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
    public void EnqueueParticipant(long participantId)
    {
        // TODO: method to reconsider using event sourcing
        // Thus we will not only move the logic of the available position calculation to database, but also ensure concurrency
        throw new NotImplementedException();
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
    public void EnqueueParticipantAt(long participantId, uint position)
    {
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

        AddDomainEvent(new ParticipantEnqueuedAtEvent(GroupId, queueName: Name, participantId, position));
    }

    public void DequeueParticipant(long participantId)
    {
        var participant = _participants.Values.FirstOrDefault(p => p.Id == participantId);
        if (participant == null || !_participants.Remove(participant.Position.Number))
        {
            throw new ParticipantDoesNotExistException(
                $"Participant '{participantId}' does not exist in the queue '{Name}'.");
        }

        AddDomainEvent(new ParticipantDequeuedEvent(GroupId, queueName: Name, participantId));
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

    private readonly struct ParticipantKey(long id, uint position) : IEquatable<ParticipantKey>
    {
        private readonly long _id = id;
        private readonly uint _position = position;

        public bool Equals(ParticipantKey other)
        {
            return _id == other._id || _position == other._position;
        }

        public override bool Equals(object? obj)
        {
            return obj is ParticipantKey key && Equals(key);
        }

        public override int GetHashCode()
        {
            return 1; // Skip hash code comparison
        }
    }
}
