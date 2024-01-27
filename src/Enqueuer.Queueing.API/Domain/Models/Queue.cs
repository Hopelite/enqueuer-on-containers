using Enqueuer.Queueing.API.Domain.Exceptions;
using System.Diagnostics.CodeAnalysis;

namespace Enqueuer.Queueing.API.Domain.Models;

/// <summary>
/// The aggregate model representing an ordered sequence of participants.
/// </summary>
/// <remarks>
/// Not thread safe. Intended to be used in a single thread.
/// The concurrency must be handled by external code.
/// </remarks>
internal class Queue
{
    private static readonly PositionComparer ReservedPositionComparer = new();
    private readonly HashSet<Participant> _participants;

    public Queue(int id)
    {
        Id = id;
        _participants = new(new IdentityComparer());
    }

    /// <summary>
    /// The unique identifier of the queue entity.
    /// </summary>
    public int Id { get; private set; }

    /// <summary>
    /// The ordered sequence of participants.
    /// </summary>
    public IReadOnlyCollection<Participant> Participants => _participants;

    /// <summary>
    /// Adds the <paramref name="participant"/> to the queue.
    /// </summary>
    /// <param name="participant">The participant to place in queue.</param>
    /// <remarks>
    /// Implemented as a separate method instead of making <see cref="Participants"/>
    /// of <see cref="ICollection{T}"/> type to enforce the CQRS pattern.
    /// </remarks>
    /// <exception cref="PositionReservedException">Thrown, if the <paramref name="participant"/>'s position is reserved.</exception>
    /// <exception cref="ParticipantAlreadyExistsException">Thrown, if the <paramref name="participant"/> already exists in the queue.</exception>
    public void EnqueueParticipant(Participant participant)
    {
        if (_participants.Contains(participant, ReservedPositionComparer))
        {
            throw new PositionReservedException(
                $"Cannot enqueue participant '{participant.Id}' to the reserved position '{participant.Position}' in the queue '{Id}'.");
        }

        if (!_participants.Add(participant))
        {
            throw new ParticipantAlreadyExistsException(
                $"Participant '{participant.Id}' already exists in the queue '{Id}'.");
        }
    }

    /// <summary>
    /// Removes the <paramref name="participant"/> from the queue.
    /// </summary>
    /// <param name="participant">The participant to remove from queue.</param>
    /// <exception cref="ParticipantDoesNotExistException">Thrown, if the <paramref name="participant"/> does not exist in the queue.</exception>
    public void DequeueParticipant(Participant participant)
    {
        if (!_participants.Remove(participant))
        {
            throw new ParticipantDoesNotExistException(
                $"Participant '{participant.Id}' does not exist in the queue '{Id}'.");
        }
    }

    private class PositionComparer : IEqualityComparer<Participant>
    {
        public bool Equals(Participant? first, Participant? second)
        {
            return (first, second) switch
            {
                (null, null) => true,
                (null, _) => false,
                (_, null) => false,
                _ => first.Position == second.Position,
            };
        }

        public int GetHashCode([DisallowNull] Participant participant)
        {
            return participant.Position.GetHashCode();
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
