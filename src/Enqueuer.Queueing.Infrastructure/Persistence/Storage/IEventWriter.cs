using Enqueuer.Queueing.Domain.Models;

namespace Enqueuer.Queueing.Infrastructure.Persistence.Storage;

public interface IEventWriter
{
    Task ApplyEventsAsync(IGroupAggregate group, CancellationToken cancellationToken);
}
