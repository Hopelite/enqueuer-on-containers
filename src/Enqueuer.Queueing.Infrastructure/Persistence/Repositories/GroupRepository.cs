using Enqueuer.Queueing.Domain.Models;
using Enqueuer.Queueing.Domain.Repositories;
using Enqueuer.Queueing.Infrastructure.Persistence.Storage;
using Enqueuer.Queueing.Infrastructure.Persistence.Storage.Helpers;

namespace Enqueuer.Queueing.Infrastructure.Persistence.Repositories;

public class GroupRepository : IGroupRepository
{
    private readonly IAggregateRootBuilder<Group> _groupBuilder;
    private readonly IEventStorage _eventStorage;

    public GroupRepository(IEventStorage eventStorage, IAggregateRootBuilder<Group> groupBuilder)
    {
        _eventStorage = eventStorage;
        _groupBuilder = groupBuilder;
    }

    public async Task<IGroupAggregate> GetOrCreateGroupAsync(long groupId, CancellationToken cancellationToken)
    {
        var groupEvents = await _eventStorage.GetAggregateEventsAsync(groupId, cancellationToken);
        var group = _groupBuilder.Build(groupId, groupEvents);

        return group;
    }
}
