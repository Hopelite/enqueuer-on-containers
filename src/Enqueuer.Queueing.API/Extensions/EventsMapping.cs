using AutoMapper;
using Enqueuer.EventBus.Abstractions;

namespace Enqueuer.Queueing.API.Extensions;

public static class EventsMapping
{
    public static IMapperConfigurationExpression MapDomainEvent<TDomain, TMessagingEvent>(this IMapperConfigurationExpression configuration)
        where TDomain: Domain.Events.DomainEvent
        where TMessagingEvent: IIntegrationEvent
    {
        configuration.CreateMap<TDomain, TMessagingEvent>();
        configuration.CreateMap<TDomain, IIntegrationEvent>().As<TMessagingEvent>();
        return configuration;
    }
}
