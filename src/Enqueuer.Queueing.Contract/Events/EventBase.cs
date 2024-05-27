using Enqueuer.EventBus.Abstractions;
using System;

namespace Enqueuer.Queueing.Contract.V1.Events
{
    public abstract class EventBase : IIntegrationEvent
    {
        // TODO: consider to pass arguments to this ctor to not loose during serialization
        protected EventBase()
        {
            Id = Guid.NewGuid();
            Timestamp = DateTime.UtcNow;
        }

        public Guid Id { get; }

        public DateTime Timestamp { get; }
    }
}
