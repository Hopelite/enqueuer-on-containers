using Enqueuer.Queueing.Domain.Events;
using Enqueuer.Queueing.Domain.Factories;
using Enqueuer.Queueing.Domain.Models;

namespace Enqueuer.Queueing.Infrastructure.Persistence.Storage;

public class GroupAggregateBuilder : IAggregateRootBuilder<Group>
{
    private readonly IGroupFactory _groupFactory;

    public GroupAggregateBuilder(IGroupFactory groupFactory)
    {
        _groupFactory = groupFactory;
    }

    public Group Build(long aggregateId, IEnumerable<DomainEvent> domainEvents)
    {
        var group = _groupFactory.Create(aggregateId);
        foreach (var @event in domainEvents)
        {
            group.Apply(@event);
        }

        return group;
    }
}
