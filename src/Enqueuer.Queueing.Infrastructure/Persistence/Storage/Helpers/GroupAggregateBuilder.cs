using Enqueuer.Queueing.Domain.Events;
using Enqueuer.Queueing.Domain.Factories;
using Enqueuer.Queueing.Domain.Models;

namespace Enqueuer.Queueing.Infrastructure.Persistence.Storage.Helpers;

public class GroupAggregateBuilder(IGroupFactory groupFactory) : IAggregateRootBuilder<Group>
{
    private readonly IGroupFactory _groupFactory = groupFactory;

    public Group Build(long aggregateId, IEnumerable<DomainEvent> domainEvents)
    {
        var group = _groupFactory.Create(aggregateId);
        foreach (var @event in domainEvents)
        {
            @event.ApplyTo(group);
        }

        return group;
    }
}
