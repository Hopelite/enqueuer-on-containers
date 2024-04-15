using AutoMapper;
using Enqueuer.EventBus.Abstractions;
using Enqueuer.Queueing.Domain.Events;
using Enqueuer.Queueing.Infrastructure.Messaging;

namespace Enqueuer.Queueing.API.Application.Messaging;

public class BusEventPublisher(IMapper mapper/*, IEventBusClient busClient*/, ILogger<BusEventPublisher> logger) : IEventPublisher
{
    private readonly IMapper _mapper = mapper;
    private readonly IEventBusClient _busClient/* = busClient*/;
    private readonly ILogger<BusEventPublisher> _logger = logger;

    public async Task PublishEventAsync(DomainEvent @event, CancellationToken cancellationToken)
    {
        try
        {
            var message = _mapper.Map<IIntegrationEvent>(@event);
            await _busClient.PublishAsync(message, cancellationToken);
        }
        catch (Exception ex)
        {
            // Communication errors must not break the application
            _logger.LogError(ex, "An error occured during the '{EventName}' event publication.", @event.Name);
        }
    }
}
