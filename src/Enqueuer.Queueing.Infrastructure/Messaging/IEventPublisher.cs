using Enqueuer.Queueing.Domain.Events;

namespace Enqueuer.Queueing.Infrastructure.Messaging;

public interface IEventPublisher
{
    Task PublishEventAsync(DomainEvent @event, CancellationToken cancellationToken);
}
