using Enqueuer.Queueing.Domain.Models;
using Enqueuer.Queueing.Infrastructure.Messaging;
using Enqueuer.Queueing.Infrastructure.Persistence.Storage;
using Enqueuer.Queueing.Infrastructure.Persistence.Storage.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Enqueuer.Queueing.Infrastructure.Commands.Handling;

public class GroupCommandHandlerFactory : ICommandHandlerFactory<Group>
{
    private readonly IServiceProvider _serviceProvider;

    public GroupCommandHandlerFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public ICommandHandler<Group> CreateCommandHandlerFor(long aggregateId)
    {
        var eventStorage = _serviceProvider.GetRequiredService<IEventStorage>();
        var aggregateBuilder = _serviceProvider.GetRequiredService<IAggregateRootBuilder<Group>>();
        var eventPublisher = _serviceProvider.GetRequiredService<IEventPublisher>();
        var logger = _serviceProvider.GetRequiredService<ILogger<CommandHandler>>();

        return new CommandHandler(aggregateId, eventStorage, aggregateBuilder, eventPublisher, logger);
    }
}
