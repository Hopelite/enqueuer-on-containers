namespace Enqueuer.Queueing.Domain.Models;

/// <summary>
/// The entity representing a reserved position in a queue.
/// </summary>
public readonly struct Position
{
    public Position(uint number)
    {
        Number = number;
    }

    /// <summary>
    /// The unique posion number in queue.
    /// </summary>
    public uint Number { get; }
}
