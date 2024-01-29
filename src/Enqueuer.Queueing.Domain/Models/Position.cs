namespace Enqueuer.Queueing.Domain.Models;

/// <summary>
/// The entity representing a reserved position in a queue.
/// </summary>
public record Position
{
    public Position(uint number, int queueId)
    {
        Number = number;
        QueueId = queueId;
    }

    /// <summary>
    /// The unique posion number in queue.
    /// </summary>
    public uint Number { get; }

    /// <summary>
    /// The unique identifier of the queue the position is reserved.
    /// </summary>
    public int QueueId { get; }
}
