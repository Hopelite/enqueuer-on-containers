using Enqueuer.EventBus.Abstractions;
using System;

namespace Enqueuer.Queueing.Contract.V1.Events
{
    public abstract class EventBase : IIntegrationEvent
    {
        protected EventBase()
        {
            Id = Guid.NewGuid();
            Timestamp = DateTime.Now;
        }

        public Guid Id { get; }

        public DateTime Timestamp { get; }
    }
}
