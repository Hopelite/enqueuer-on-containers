using System;
using Enqueuer.EventBus.Abstractions;

namespace Enqueuer.Queueing.Contract.V1.Events
{
    public abstract class EventBase : IIntegrationEvent
    {
        // TODO: consider to pass arguments to this ctor to not loose during serialization
        protected EventBase()
        {
            Id = Guid.NewGuid();
            Timestamp = DateTime.Now;
        }

        public Guid Id { get; }

        public DateTime Timestamp { get; }

        public abstract string Name { get; }
    }
}
