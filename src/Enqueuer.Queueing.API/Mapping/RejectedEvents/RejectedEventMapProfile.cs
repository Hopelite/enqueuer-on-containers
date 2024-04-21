using AutoMapper;
using Enqueuer.EventBus.Abstractions;
using Enqueuer.Queueing.Infrastructure.Messaging;

namespace Enqueuer.Queueing.API.Mapping.RejectedEvents;

public class RejectedEventMapProfile : Profile
{
    public RejectedEventMapProfile()
    {
        CreateMap<RejectedEvent, IIntegrationEvent>().ConvertUsing(new RejectedEventsTypeResolver());
    }
}
