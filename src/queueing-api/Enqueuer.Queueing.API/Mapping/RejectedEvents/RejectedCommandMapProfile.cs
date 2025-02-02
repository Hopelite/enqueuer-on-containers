using AutoMapper;
using Enqueuer.EventBus.Abstractions;
using Enqueuer.Queueing.Infrastructure.Messaging;

namespace Enqueuer.Queueing.API.Mapping.RejectedEvents;

public class RejectedCommandMapProfile : Profile
{
    public RejectedCommandMapProfile()
    {
        CreateMap<RejectedCommand, IIntegrationEvent>().ConvertUsing(new RejectedCommandTypeResolver());
    }
}
