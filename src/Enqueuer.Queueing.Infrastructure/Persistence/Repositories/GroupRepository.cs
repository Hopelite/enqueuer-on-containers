using Enqueuer.Queueing.Domain.Models;
using Enqueuer.Queueing.Domain.Repositories;
using Enqueuer.Queueing.Infrastructure.Persistence.Storage;

namespace Enqueuer.Queueing.Infrastructure.Persistence.Repositories;

public class GroupRepository : IGroupRepository
{
    private readonly IEventWriterManager<Group> _eventWriterManager;
    private readonly IAggregateRootBuilder<Group> _groupBuilder;
    private readonly IEventStorage _eventStorage;

    public GroupRepository(
        IEventStorage eventStorage,
        IEventWriterManager<Group> eventWriterManager,
        IAggregateRootBuilder<Group> groupBuilder)
    {
        _eventStorage = eventStorage;
        _eventWriterManager = eventWriterManager;
        _groupBuilder = groupBuilder;
    }

    public async Task<IGroupAggregate> GetGroupAsync(long groupId, CancellationToken cancellationToken)
    {
        var groupEvents = await _eventStorage.GetAggregateEventsAsync(groupId, cancellationToken);
        var group = _groupBuilder.Build(groupId, groupEvents);

        return group;
    }

    public async Task SaveChangesAsync(IGroupAggregate group, CancellationToken cancellationToken)
    {
        var aggregateRoot = (Group)group;

        var domainEvents = aggregateRoot.DomainEvents.Concat(
            aggregateRoot.Queues.SelectMany(q => q.DomainEvents))
                .OrderBy(e => e.Timestamp);

        var writer = await _eventWriterManager.GetActiveEventWriterForAsync(aggregateRoot.Id);
        await writer.WriteEventsAsync(domainEvents, cancellationToken);
    }
}
