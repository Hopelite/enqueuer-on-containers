using System.Threading;
using System.Threading.Tasks;

namespace Enqueuer.EventBus.Abstractions
{
    /// <summary>
    /// Defines the way for clients to asynchronically communicate with each other.
    /// </summary>
    public interface IEventBusClient
    {
        /// <summary>
        /// Publishes the <paramref name="event"/> via bus connection.
        /// </summary>
        Task PublishAsync(IIntegrationEvent @event, CancellationToken cancellationToken);
    }
}
