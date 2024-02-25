using Enqueuer.Queueing.Domain.Factories;
using Enqueuer.Queueing.Domain.Models;
using Enqueuer.Queueing.Domain.Repositories;
using Enqueuer.Queueing.Infrastructure.Persistence.Storage;

namespace Enqueuer.Queueing.Infrastructure.Persistence.Repositories;

public class GroupRepository : IGroupRepository
{
    private readonly IEventStorage _eventStorage;
    private readonly IGroupFactory _groupFactory;

    public GroupRepository(
        IEventStorage eventStorage,
        IGroupFactory groupFactory)
    {
        _eventStorage = eventStorage;
        _groupFactory = groupFactory;
    }

    public async Task<IGroupAggregate> GetGroupAsync(long groupId, CancellationToken cancellationToken)
    {
        var groupEvents = await _eventStorage.GetAggregateEventsAsync(groupId, cancellationToken);
        var group = _groupFactory.Create(groupId);

        foreach (var @event in groupEvents)
        {
            group.Apply(@event);
        }

        return group;
    }

    public Task SaveChangesAsync(IGroupAggregate group, CancellationToken cancellationToken)
    {
        var domainEvents = ((Group)group).DomainEvents.Concat(
            ((Group)group).Queues.SelectMany(q => q.DomainEvents))
            .OrderBy(e => e.Timestamp);

        return _eventStorage.WriteEventsAsync(domainEvents, cancellationToken);
    }
}
