using Enqueuer.Queueing.Domain.Models;

namespace Enqueuer.Queueing.Domain.Repositories;

public interface IGroupRepository
{
    /// <summary>
    /// Gets the existing or creates new group with the specified <paramref name="groupId"/>.
    /// </summary>
    Task<IGroupAggregate> GetOrCreateGroupAsync(long groupId, CancellationToken cancellationToken);
}
