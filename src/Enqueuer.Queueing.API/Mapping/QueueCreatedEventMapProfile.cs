using AutoMapper;
using Enqueuer.EventBus.Abstractions;

namespace Enqueuer.Queueing.API.Mapping;

public class QueueCreatedEventMapProfile : Profile
{
    public QueueCreatedEventMapProfile()
    {
        CreateMap<Domain.Events.QueueCreatedEvent, Contract.V1.Events.QueueCreatedEvent>()
            .ConstructUsing(e => new Contract.V1.Events.QueueCreatedEvent(e.AggregateId, e.QueueName));

        CreateMap<Domain.Events.QueueCreatedEvent, IIntegrationEvent>().As<Contract.V1.Events.QueueCreatedEvent>();
    }
}
