namespace Enqueuer.Queueing.Domain.Repositories;

/// <summary>
/// Defines the way to save the tracked entities.
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Saves the changes with tracked entities to data storage.
    /// </summary>
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
