using AutoMapper;
using Enqueuer.EventBus.Abstractions;

namespace Enqueuer.Queueing.API.Mapping;

public class ParticipantDequeuedEventMapProfile : Profile
{
    public ParticipantDequeuedEventMapProfile()
    {
        CreateMap<Domain.Events.ParticipantDequeuedEvent, Contract.V1.Events.ParticipantDequeuedEvent>()
            .ConstructUsing(e => new Contract.V1.Events.ParticipantDequeuedEvent(e.AggregateId, e.QueueName, e.ParticipantId));

        CreateMap<Domain.Events.ParticipantDequeuedEvent, IIntegrationEvent>().As<Contract.V1.Events.ParticipantDequeuedEvent>();
    }
}
