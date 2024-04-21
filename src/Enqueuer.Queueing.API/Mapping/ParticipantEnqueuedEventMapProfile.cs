using AutoMapper;
using Enqueuer.EventBus.Abstractions;

namespace Enqueuer.Queueing.API.Mapping;

public class ParticipantEnqueuedEventMapProfile : Profile
{
    public ParticipantEnqueuedEventMapProfile()
    {
        CreateMap<Domain.Events.ParticipantEnqueuedEvent, Contract.V1.Events.ParticipantEnqueuedEvent>()
            .ConstructUsing(e => new Contract.V1.Events.ParticipantEnqueuedEvent(e.AggregateId, e.QueueName, e.ParticipantId));

        CreateMap<Domain.Events.ParticipantEnqueuedEvent, IIntegrationEvent>().As<Contract.V1.Events.ParticipantEnqueuedEvent>();
    }
}
