using Enqueuer.Queueing.Domain.Models;
using Enqueuer.Queueing.Infrastructure.Messaging;
using Enqueuer.Queueing.Infrastructure.Persistence.Storage.Helpers;
using Enqueuer.Queueing.Infrastructure.Persistence.Storage.Writing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Enqueuer.Queueing.Infrastructure.Persistence.Storage;

public class GroupEventWriterFactory : IEventWriterFactory<Group>
{
    private readonly IServiceProvider _serviceProvider;

    public GroupEventWriterFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IEventWriter<Group> CreateEventWriterFor(long aggregateId)
    {
        var eventStorage = _serviceProvider.GetRequiredService<IEventStorage>();
        var aggregateBuilder = _serviceProvider.GetRequiredService<IAggregateRootBuilder<Group>>();
        var eventPublisher = _serviceProvider.GetRequiredService<IEventPublisher>();
        var logger = _serviceProvider.GetRequiredService<ILogger<EventWriter>>();

        return new EventWriter(aggregateId, eventStorage, aggregateBuilder, eventPublisher, logger);
    }
}
