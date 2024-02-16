using Enqueuer.EventBus.Abstractions;
using System;

namespace Enqueuer.Queueing.Contract.V1.Events
{
    public abstract class EventBase : IIntegrationEvent
    {
        public EventBase()
        {
            Id = Guid.NewGuid();
            CreationDate = DateTime.Now;
        }

        public Guid Id { get; }

        public DateTime CreationDate { get; }
    }
}
