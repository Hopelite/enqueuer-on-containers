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

    /// <summary>
    /// Gets the first non-reserved <see cref="Position"/> out of <paramref name="reservedPositions"/>.
    /// </summary>
    public static Position GetFirstAvailablePosition(ICollection<uint> reservedPositions)
    {
        uint firstAvailablePosition = 1;
        for (uint i = 1; i <= reservedPositions.Count + 1; i++)
        {
            if (!reservedPositions.Contains(i))
            {
                firstAvailablePosition = i;
                break;
            }
        }

        return new Position(firstAvailablePosition);
    }
}
