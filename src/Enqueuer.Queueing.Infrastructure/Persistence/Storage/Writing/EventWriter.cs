using Enqueuer.Queueing.Domain.Events;
using Enqueuer.Queueing.Domain.Exceptions;
using Enqueuer.Queueing.Domain.Models;
using Enqueuer.Queueing.Infrastructure.Messaging;
using Enqueuer.Queueing.Infrastructure.Persistence.Storage.Helpers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Channels;

namespace Enqueuer.Queueing.Infrastructure.Persistence.Storage;

internal class EventWriter : BackgroundService, IEventWriter<Group>
{
    private readonly IAggregateRootBuilder<Group> _aggregateBuilder;
    private readonly Channel<DomainEvent> _eventsToWrite;
    private readonly IEventPublisher _eventPublisher;
    private readonly ILogger<EventWriter> _logger;
    private readonly IEventStorage _eventStorage;
    private readonly long _aggregateId;

    public EventWriter(
        long aggregateId,
        IEventStorage eventStorage,
        IAggregateRootBuilder<Group> aggregateBuilder,
        IEventPublisher eventPublisher,
        ILogger<EventWriter> logger)
    {
        _eventsToWrite = Channel.CreateUnbounded<DomainEvent>();
        _aggregateId = aggregateId;
        _eventStorage = eventStorage;
        _aggregateBuilder = aggregateBuilder;
        _eventPublisher = eventPublisher;
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

        _logger.LogInformation("Started {EventWriter} for aggregate root '{AggregateId}'.", nameof(EventWriter), _aggregateId);
        await foreach (var @event in _eventsToWrite.Reader.ReadAllAsync(stoppingToken))
        {
            try
            {
                @event.ApplyTo(group);
            }
            catch (DomainException ex)
            {
                _logger.LogInformation(ex, "Rejected event '{EventName}' for the group '{GroupId}'.", @event.Name, _aggregateId);
                await TryPublishEventRejectedAsync(@event, ex, stoppingToken);
                continue;
            }

            try
            {
                await _eventStorage.WriteEventAsync(@event, stoppingToken);
                await _eventPublisher.PublishEventAsync(@event, stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "A non-domain exception was thrown during the '{EventName}' writing for the group '{GroupId}'.", @event.Name, _aggregateId);
                await TryPublishEventRejectedAsync(@event, ex, stoppingToken);
            }
        }

        _logger.LogInformation("Stopped {EventWriter} for aggregate root '{AggregateId}'.", nameof(EventWriter), _aggregateId);
    }

    private Task TryPublishEventRejectedAsync(DomainEvent rejectedEvent, Exception exception, CancellationToken cancellationToken)
    {
        try
        {
            //return _eventPublisher.PublishEventAsync(new RejectedEvent(rejectedEvent, exception), cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to notify about rejected '{EventName}' for the group '{GroupId}'.", rejectedEvent.Name, _aggregateId);
        }

        return Task.CompletedTask;
    }
}
