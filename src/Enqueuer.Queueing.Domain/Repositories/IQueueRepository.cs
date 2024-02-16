using Enqueuer.Queueing.Domain.Models;

namespace Enqueuer.Queueing.Domain.Repositories;

/// <summary>
/// Defines the way to operate with <see cref="Queue"/> in data storage.
/// </summary>
public interface IQueueRepository : IUnitOfWork
{
    /// <summary>
    /// Gets the existing queue with the specified <paramref name="id"/>.
    /// </summary>
    Task<Queue> GetQueueAsync(long id, CancellationToken cancellationToken);

    /// <summary>
    /// Creates new and starts tracking <see cref="Queue"/> with the specified <paramref name="name"/> and <paramref name="groupId"/>.
    /// </summary>
    Queue CreateNewQueue(string name, long groupId);

    /// <summary>
    /// Updates the existing <paramref name="queue"/> in storage.
    /// </summary>
    void UpdateQueue(Queue queue);

    /// <summary>
    /// Deletes the existing queue with the specified <paramref name="id"/>.
    /// </summary>
    void DeleteQueue(Queue queue);
}
