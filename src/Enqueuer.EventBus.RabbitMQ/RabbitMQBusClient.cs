using Enqueuer.EventBus.Abstractions;

namespace Enqueuer.EventBus.RabbitMQ;

public class RabbitMQBusClient : IEventBusClient
{
    public Task PublishAsync(IIntegrationEvent @event, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
