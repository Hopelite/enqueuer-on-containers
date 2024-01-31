using Enqueuer.Queueing.Domain.Events;

namespace Enqueuer.Queueing.Infrastructure.Messaging;

public interface IEventDispatcher
{
    Task DispatchEventsAsync(IReadOnlyCollection<DomainEvent> events, CancellationToken cancellationToken);
}
