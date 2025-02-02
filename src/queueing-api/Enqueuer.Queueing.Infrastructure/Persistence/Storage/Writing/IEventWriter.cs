using Enqueuer.Queueing.Domain.Events;
using Microsoft.Extensions.Hosting;

namespace Enqueuer.Queueing.Infrastructure.Persistence.Storage;

/// <summary>
/// Provides functionality to write events for the <typeparamref name="TAggregate"/>.
/// </summary>
public interface IEventWriter<TAggregate> : IHostedService
{
    /// <summary>
    /// Writes <paramref name="events"/> to persistent storage.
    /// </summary>
    Task WriteEventsAsync(IEnumerable<DomainEvent> events, CancellationToken cancellationToken);
}
