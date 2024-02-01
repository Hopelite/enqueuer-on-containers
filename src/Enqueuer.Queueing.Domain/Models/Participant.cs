namespace Enqueuer.Queueing.Domain.Models;

/// <summary>
/// The entity representing an owner of a position in a queue.
/// </summary>
public class Participant
{
    public Participant(long id, uint number)
        : this(id, new Position(number))
    {
    }

    internal Participant(long id, Position position)
    {
        Id = id;
        Position = position;
    }

    /// <summary>
    /// The unique identifier of the participant entity.
    /// </summary>
    public long Id { get; }

    /// <summary>
    /// The position the participant owns.
    /// </summary>
    public Position Position { get; }
}
