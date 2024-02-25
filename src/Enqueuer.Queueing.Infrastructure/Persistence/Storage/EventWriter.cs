using Enqueuer.Queueing.Domain.Events;
using Enqueuer.Queueing.Domain.Exceptions;
using Enqueuer.Queueing.Domain.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Channels;

namespace Enqueuer.Queueing.Infrastructure.Persistence.Storage;

internal class EventWriter : BackgroundService, IEventWriter<Group>
{
    private readonly Channel<DomainEvent> _eventsToWrite;
    private readonly IAggregateRootBuilder<Group> _aggregateBuilder;
    private readonly ILogger<EventWriter> _logger;
    private readonly IEventStorage _eventStorage;
    private readonly long _aggregateId;

    public EventWriter(
        long aggregateId,
        IEventStorage eventStorage,
        IAggregateRootBuilder<Group> aggregateBuilder,
        ILogger<EventWriter> logger)
    {
        _eventsToWrite = Channel.CreateUnbounded<DomainEvent>();
        _aggregateId = aggregateId;
        _eventStorage = eventStorage;
        _aggregateBuilder = aggregateBuilder;
        _logger = logger;
    }

    public async Task WriteEventsAsync(IEnumerable<DomainEvent> events, CancellationToken cancellationToken)
    {
        foreach (var @event in events)
        {
            await _eventsToWrite.Writer.WriteAsync(@event, cancellationToken);
        }
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var existingEvents = await _eventStorage.GetAggregateEventsAsync(_aggregateId, stoppingToken);
        var group = _aggregateBuilder.Build(_aggregateId, existingEvents);

        _logger.LogInformation("Starting {EventWriter} for aggregate root '{AggregateId}'.", nameof(EventWriter), _aggregateId);
        await foreach (var @event in _eventsToWrite.Reader.ReadAllAsync(stoppingToken))
        {
            try
            {
                // TODO: apply and notify about events here
                group.Apply(@event);
                await _eventStorage.WriteEventAsync(@event, stoppingToken);
            }
            catch (DomainException ex)
            {
                // TODO: notify about rejected event
                _logger.LogInformation(ex, "Rejected event '{EventName}' for the group '{GroupId}'.", @event.Name, _aggregateId);
            }
            catch (Exception ex)
            {
                // TODO: possibly notify about rejected event
                _logger.LogError(ex, "A non-domain exception was thrown during the '{EventName}' writing for the group '{GroupId}'.", @event.Name, _aggregateId);
            }
        }

        _logger.LogInformation("Stopping {EventWriter} for aggregate root '{AggregateId}'.", nameof(EventWriter), _aggregateId);
    }
}
