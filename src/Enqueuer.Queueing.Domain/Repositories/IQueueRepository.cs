using Enqueuer.Queueing.Domain.Models;

namespace Enqueuer.Queueing.Domain.Repositories;

/// <summary>
/// Defines the way to operate with <see cref="Queue"/> in data storage.
/// </summary>
public interface IQueueRepository : IUnitOfWork
{
    /// <summary>
    /// Gets the existing queue with the specified <paramref name="id"/>
    /// </summary>
    /// <returns></returns>
    Task<Queue> GetQueueAsync(int id, CancellationToken cancellationToken);

    /// <summary>
    /// Creates new and starts tracking <see cref="Queue"/> with the specified <paramref name="name"/> and <paramref name="locationId"/>.
    /// </summary>
    Queue CreateNewQueue(string name, long locationId);

    /// <summary>
    /// Updates the existing <paramref name="queue"/> in storage.
    /// </summary>
    void UpdateQueue(Queue queue);

    /// <summary>
    /// Deletes the existing queue with the specified <paramref name="id"/>.
    /// </summary>
    void DeleteQueue(Queue queue);
}
