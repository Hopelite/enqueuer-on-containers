using AutoMapper;
using Enqueuer.EventBus.Abstractions;
using Enqueuer.Queueing.Contract.V1.Events.RejectedEvents;
using Enqueuer.Queueing.Domain.Exceptions;

namespace Enqueuer.Queueing.API.Mapping.RejectedEvents;

public class RejectedCommandTypeResolver : ITypeConverter<Infrastructure.Messaging.RejectedCommand, IIntegrationEvent>
{
    public IIntegrationEvent Convert(Infrastructure.Messaging.RejectedCommand source, IIntegrationEvent destination, ResolutionContext context)
    {
        return source.Exception switch
        {
            QueueAlreadyExistsException e => new QueueAlreadyExistsEvent(source.AggregateId, e.QueueName),
            QueueDoesNotExistException e => new QueueDoesNotExistEvent(source.AggregateId, e.QueueName),
            ParticipantAlreadyExistsException e => new ParticipantAlreadyExistsEvent(source.AggregateId, e.QueueName, e.ParticipantId),
            PositionReservedException e => new PositionIsReservedEvent(source.AggregateId, e.QueueName, e.Position),
            Exception e => new RejectedEvent(source.AggregateId, e.Message)
        };
    }
}
