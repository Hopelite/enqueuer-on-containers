using AutoMapper;
using Enqueuer.EventBus.Abstractions;

namespace Enqueuer.Queueing.API.Extensions;

public static class EventsMapping
{
    public static IMapperConfigurationExpression MapDomainEvent<TDomainEvent, TMessagingEvent>(this IMapperConfigurationExpression configuration)
        where TDomainEvent: Domain.Events.DomainEvent
        where TMessagingEvent: IIntegrationEvent
    {
        configuration.CreateMap<TDomainEvent, TMessagingEvent>();
        configuration.CreateMap<TDomainEvent, IIntegrationEvent>().As<TMessagingEvent>();
        return configuration;
    }

    public static IMapperConfigurationExpression MapRejectedDomainEvent<TRejectedEvent, TMessagingEvent>(this IMapperConfigurationExpression configuration)
        where TRejectedEvent : Infrastructure.Messaging.RejectedEvent
        where TMessagingEvent : IIntegrationEvent
    {
        //configuration.CreateMap<TRejectedEvent, TMessagingEvent>()
        //    .

        //configuration.CreateMap<TRejectedEvent, IIntegrationEvent>().As<TMessagingEvent>();
        return configuration;
    }
}
