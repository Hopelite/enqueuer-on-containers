namespace Enqueuer.Queueing.API.Domain.Models;

/// <summary>
/// The entity representing a member of a queue.
/// </summary>
/// <remarks>Combines the "Booking" and "User" models from the other Bounded Contexts.</remarks>
public class Participant
{
    /// <summary>
    /// The unique identifier of the participant entity.
    /// </summary>
    public long Id { get; init; }

    /// <summary>
    /// The unique identifier of the queue the participant is placed.
    /// </summary>
    public int QueueId { get; init; }

    /// <summary>
    /// The participant's position in queue.
    /// </summary>
    public uint Position { get; init; }
}
