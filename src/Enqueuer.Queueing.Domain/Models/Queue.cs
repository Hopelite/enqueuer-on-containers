using Enqueuer.Queueing.Domain.Events;
using Enqueuer.Queueing.Domain.Exceptions;
using System.Diagnostics.CodeAnalysis;

namespace Enqueuer.Queueing.Domain.Models;

/// <summary>
/// The aggregate root model representing an ordered sequence of participants.
/// </summary>
/// <remarks>
/// Not thread safe. Intended to be used in a single thread.
/// The concurrency must be handled by external code.
/// </remarks>
public class Queue : Entity
{
    private static readonly IdentityComparer ParticipantIdentityComparer = new();
    private readonly Dictionary<uint, Participant> _participants;
    private string _name = null!;

    internal Queue(long id, string name, long groupId)
    {
        Id = id;
        Name = name;
        GroupId = groupId;
        _participants = new();
    }

    /// <summary>
    /// The unique identifier of the queue entity.
    /// </summary>
    public long Id { get; }

    /// <summary>
    /// The name of the queue.
    /// </summary>
    public string Name
    {
        get => _name;
        private set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new InvalidQueueNameException("Queue name can't be null, empty or a whitespace.");
            }

            if (value.Length > QueueLimits.MaxNameLength)
            {
                throw new InvalidQueueNameException($"Queue name can't be longer than {QueueLimits.MaxNameLength} symbols.");
            }

            _name = value;
        }
    }

    /// <summary>
    /// The unique identifier of the group this queue is related.
    /// </summary>
    public long GroupId { get; }

    /// <summary>
    /// The ordered sequence of reserved positions.
    /// </summary>
    public IReadOnlyCollection<Participant> Participants => _participants.Values;

    /// <summary>
    /// Adds the participant with the specified <paramref name="participantId"/>
    /// at the specified <paramref name="position"/> in queue.
    /// </summary>
    /// <param name="participant">The participant to place in queue.</param>
    /// <remarks>
    /// Implemented as a separate method instead of making <see cref="Participants"/>
    /// of <see cref="ICollection{T}"/> type to enforce the CQRS pattern.
    /// </remarks>
    /// <exception cref="PositionReservedException">Thrown, if the <paramref name="participant"/>'s position is reserved.</exception>
    /// <exception cref="ParticipantAlreadyExistsException">Thrown, if the <paramref name="participant"/> already exists in the queue.</exception>
    public void EnqueueParticipant(long participantId, uint position)
    {
        var participant = new Participant(participantId, position);
        if (_participants.Values.Contains(participant, ParticipantIdentityComparer))
        {
            throw new ParticipantAlreadyExistsException(
                $"Participant '{participant.Id}' already exists in the queue '{Id}'.");
        }

        if (!_participants.TryAdd(position, participant))
        {
            throw new PositionReservedException(
                $"Cannot enqueue participant '{participant.Id}' to the reserved position '{participant.Position}' in the queue '{Id}'.");
        }

        // TODO: notify about enqueued participant
    }

    public void DequeueParticipant(long participantId)
    {
        var participant = _participants.Values.FirstOrDefault(p => p.Id == participantId);
        if (participant == null || !_participants.Remove(participant.Position.Number))
        {
            throw new ParticipantDoesNotExistException(
                $"Participant '{participantId}' does not exist in the queue '{Id}'.");
        }

        // TODO: notify about dequeued participant
    }

    /// <summary>
    /// Removes participant at the specified <paramref name="position"/> from the queue.
    /// </summary>
    /// <exception cref="ParticipantDoesNotExistException">Thrown, if participant at the specified <paramref name="position"/> does not exist in the queue.</exception>
    public void DequeueParticipantAt(uint position)
    {
        if (!_participants.Remove(position, out var participant))
        {
            throw new ParticipantDoesNotExistException(
                $"Position '{position}' is not reserved in the queue '{Id}'.");
        }

        // TODO: notify about dequeued participant
    }

    /// <summary>
    /// Changes the <see cref="Name"/> to <paramref name="newName"/>.
    /// </summary>
    /// <exception cref="ArgumentNullException"></exception>
    public void ChangeName(string newName)
    {
        var oldName = Name;

        Name = newName;

        AddDomainEvent(new QueueRenamedEvent(Id, oldName, newName));
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
