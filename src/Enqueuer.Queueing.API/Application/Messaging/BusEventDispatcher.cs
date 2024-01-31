using AutoMapper;
using Enqueuer.EventBus.Abstractions;
using Enqueuer.Queueing.Domain.Events;
using Enqueuer.Queueing.Infrastructure.Messaging;

namespace Enqueuer.Queueing.API.Application.Messaging;

public class BusEventDispatcher : IEventDispatcher
{
    private readonly IMapper _mapper;
    private readonly IEventBusClient _busClient;
    private readonly ILogger<BusEventDispatcher> _logger;

    public BusEventDispatcher(IMapper mapper, IEventBusClient busClient, ILogger<BusEventDispatcher> logger)
    {
        _mapper = mapper;
        _busClient = busClient;
        _logger = logger;
    }

    public async Task DispatchEventsAsync(IReadOnlyCollection<DomainEvent> events, CancellationToken cancellationToken)
    {
        foreach (var @event in events)
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
}
