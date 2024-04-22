using AutoMapper;
using Enqueuer.EventBus.Abstractions;

namespace Enqueuer.Queueing.API.Mapping;

public class ParticipantEnqueuedAtEventMapProfile : Profile
{
    public ParticipantEnqueuedAtEventMapProfile()
    {
        CreateMap<Domain.Events.ParticipantEnqueuedAtEvent, Contract.V1.Events.ParticipantEnqueuedEvent>()
            .ConstructUsing(e => new Contract.V1.Events.ParticipantEnqueuedEvent(e.AggregateId, e.QueueName, e.ParticipantId, e.Position));

        CreateMap<Domain.Events.ParticipantEnqueuedAtEvent, IIntegrationEvent>().As<Contract.V1.Events.ParticipantEnqueuedEvent>();
    }
}
