using System.Threading;
using System.Threading.Tasks;

namespace Enqueuer.EventBus.Abstractions
{
    /// <summary>
    /// Base implementation of <see cref="IIntegrationEventHandler"/>.
    /// </summary>
    public abstract class IntegrationEventHandlerBase<TEvent> : IIntegrationEventHandler<TEvent>
        where TEvent : IIntegrationEvent
    {
        public abstract Task HandleAsync(TEvent @event, CancellationToken cancellationToken);

        public Task HandleAsync(IIntegrationEvent @event, CancellationToken cancellationToken)
        {
            return HandleAsync((TEvent)@event, cancellationToken);
        }
    }
}
