using Enqueuer.Queueing.Domain.Events;
using Enqueuer.Queueing.Domain.Exceptions;
using Enqueuer.Queueing.Domain.Factories;
using Enqueuer.Queueing.Domain.Models;
using Microsoft.Extensions.Logging;

namespace Enqueuer.Queueing.Infrastructure.Persistence.Storage.Helpers;

public class GroupAggregateBuilder(IGroupFactory groupFactory, ILogger<GroupAggregateBuilder> logger) : IAggregateRootBuilder<Group>
{
    private readonly IGroupFactory _groupFactory = groupFactory;
    private readonly ILogger<GroupAggregateBuilder> _logger = logger;

    public Group Build(long aggregateId, IEnumerable<DomainEvent> domainEvents)
    {
        var group = _groupFactory.Create(aggregateId);
        foreach (var @event in domainEvents)
        {
            try
            {
                @event.ApplyTo(group);
            }
            catch (DomainException ex)
            {
                _logger.LogCritical(ex, "An invalid event '{EventName}' with timestamp '{Timestamp}' has been stored in the event storage. Skipping it to safely build an aggregate with ID '{AggregateId}'.", @event.Name, @event.Timestamp, @event.AggregateId);
            }
        }

        return group;
    }
}
