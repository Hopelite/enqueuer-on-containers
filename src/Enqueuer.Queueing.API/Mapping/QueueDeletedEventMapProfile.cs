using AutoMapper;
using Enqueuer.EventBus.Abstractions;

namespace Enqueuer.Queueing.API.Mapping;

public class QueueDeletedEventMapProfile : Profile
{
    public QueueDeletedEventMapProfile()
    {
        CreateMap<Domain.Events.QueueDeletedEvent, Contract.V1.Events.QueueDeletedEvent>()
            .ConstructUsing(e => new Contract.V1.Events.QueueDeletedEvent(e.QueueName, e.AggregateId, 0)); // TODO: Remove this hardcoded value

        CreateMap<Domain.Events.QueueDeletedEvent, IIntegrationEvent>().As<Contract.V1.Events.QueueDeletedEvent>();
    }
}
