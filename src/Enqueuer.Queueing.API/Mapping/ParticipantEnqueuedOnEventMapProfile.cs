using AutoMapper;
using Enqueuer.EventBus.Abstractions;

namespace Enqueuer.Queueing.API.Mapping;

public class ParticipantEnqueuedOnEventMapProfile : Profile
{
    public ParticipantEnqueuedOnEventMapProfile()
    {
        CreateMap<Domain.Events.ParticipantEnqueuedOnEvent, Contract.V1.Events.ParticipantEnqueuedOnEvent>()
            .ConstructUsing(e => new Contract.V1.Events.ParticipantEnqueuedOnEvent(e.AggregateId, e.QueueName, e.ParticipantId, e.Position));

        CreateMap<Domain.Events.ParticipantEnqueuedOnEvent, IIntegrationEvent>().As<Contract.V1.Events.ParticipantEnqueuedOnEvent>();
    }
}
