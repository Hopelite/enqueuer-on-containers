using System.Threading;
using System.Threading.Tasks;

namespace Enqueuer.EventBus.Abstractions
{
    public interface IIntegrationEventHandler
    {
        Task HandleAsync(IIntegrationEvent @event, CancellationToken cancellationToken);
    }

    public interface IIntegrationEventHandler<TEvent> : IIntegrationEventHandler
        where TEvent: IIntegrationEvent
    {
        Task HandleAsync(TEvent @event, CancellationToken cancellationToken);
    }
}
