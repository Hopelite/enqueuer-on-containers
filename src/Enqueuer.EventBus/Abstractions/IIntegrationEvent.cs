using System;

namespace Enqueuer.EventBus.Abstractions
{
    /// <summary>
    /// Defiens the event being sent by bus client.
    /// </summary>
    public interface IIntegrationEvent
    {
        /// <summary>
        /// The unique identifier of the event.
        /// Can be used to avoid duplications and to store messages for resiliency.
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// The timespamp when this event was created.
        /// </summary>
        DateTime CreationDate { get; }

        /// <summary>
        /// The name of the event used by clients to listen for.
        /// </summary>
        string Name { get; }
    }
}
