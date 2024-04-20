using Enqueuer.Queueing.Domain.Models;

namespace Enqueuer.Queueing.Domain.Repositories;

public interface IGroupRepository
{
    /// <summary>
    /// Gets the existing or creates new group with the specified <paramref name="groupId"/>.
    /// </summary>
    Task<IGroupAggregate> GetOrCreateGroupAsync(long groupId, CancellationToken cancellationToken);

    /// <summary>
    /// Saves all applied to the <paramref name="group"/> changes.
    /// </summary>
    Task SaveChangesAsync(IGroupAggregate group, CancellationToken cancellationToken);
}
