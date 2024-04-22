using Enqueuer.Queueing.Domain.Exceptions;
using Enqueuer.Queueing.Domain.Models;
using Enqueuer.Queueing.Infrastructure.Messaging;
using Enqueuer.Queueing.Infrastructure.Persistence.Storage;
using Enqueuer.Queueing.Infrastructure.Persistence.Storage.Helpers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Channels;

namespace Enqueuer.Queueing.Infrastructure.Commands.Handling;

public class CommandHandler : BackgroundService, ICommandHandler
{
    private readonly IAggregateRootBuilder<Group> _aggregateBuilder;
    private readonly Channel<ICommand> _commandsToProcess;
    private readonly ILogger<CommandHandler> _logger;
    private readonly IEventPublisher _eventPublisher;
    private readonly IEventStorage _eventStorage;
    private readonly long _aggregateId;

    public CommandHandler(
        long aggregateId,
        IEventStorage eventStorage,
        IAggregateRootBuilder<Group> aggregateBuilder,
        IEventPublisher eventPublisher,
        ILogger<CommandHandler> logger)
    {
        _commandsToProcess = Channel.CreateUnbounded<ICommand>();

        _aggregateId = aggregateId;
        _eventStorage = eventStorage;
        _aggregateBuilder = aggregateBuilder;
        _eventPublisher = eventPublisher;
        _logger = logger;
    }

    public Task HandleAsync(ICommand command, CancellationToken cancellationToken)
    {
        return _commandsToProcess.Writer.WriteAsync(command, cancellationToken).AsTask();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var existingEvents = await _eventStorage.GetAggregateEventsAsync(_aggregateId, stoppingToken);
        var group = _aggregateBuilder.Build(_aggregateId, existingEvents);

        _logger.LogInformation("Started {CommandHandler} for aggregate root '{AggregateId}'.", nameof(CommandHandler), _aggregateId);
        await foreach (var command in _commandsToProcess.Reader.ReadAllAsync(stoppingToken))
        {
            try
            {
                command.Execute(group);
            }
            catch (DomainException ex)
            {
                // TODO: reject command
                // TODO: add mapping for commands 
                _logger.LogInformation(ex, "Rejected command '{CommandName}' for the group '{GroupId}'.", command.Name, _aggregateId);
                continue;
            }

            foreach (var @event in group.DomainEvents)
            {
                try
                {
                    await _eventStorage.WriteEventAsync(@event, stoppingToken);
                }
                catch (Exception ex)
                {
                    // TODO: Reject event
                    _logger.LogError(ex, "A non-domain exception was thrown during the '{EventName}' writing for the group '{GroupId}'.", @event.Name, _aggregateId);
                    continue;
                }

                try
                {
                    await _eventPublisher.PublishEventAsync(@event, stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An exception was thrown during the '{EventName}' publishing for the group '{GroupId}'.", @event.Name, _aggregateId);
                }
            }

            // TODO: refactor
            group.ClearDomainEvents();
        }
    }
}
