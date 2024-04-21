using AutoMapper;
using Enqueuer.EventBus.Abstractions;
using Enqueuer.Queueing.Contract.V1.Events.RejectedEvents;
using Enqueuer.Queueing.Domain.Exceptions;

namespace Enqueuer.Queueing.API.Mapping.RejectedEvents;

public class RejectedEventsTypeResolver : ITypeConverter<Infrastructure.Messaging.RejectedEvent, IIntegrationEvent>
{
    public IIntegrationEvent Convert(Infrastructure.Messaging.RejectedEvent source, IIntegrationEvent destination, ResolutionContext context)
    {
        return source.Exception switch
        {
            QueueAlreadyExistsException e => new QueueAlreadyExistsEvent(source.AggregateId, e.QueueName),
            QueueDoesNotExistException e => new QueueDoesNotExistEvent(source.AggregateId, e.QueueName),
            Exception e => new RejectedEvent(source.AggregateId, e.Message)
        };
    }
}
