namespace Enqueuer.Queueing.Infrastructure.Persistence.Storage;

/// <summary>
/// Defines the manager of <see cref="IEventWriter{TAggregate}"/> instances.
/// </summary>
public interface IEventWriterManager<TAggregate>
{
    /// <summary>
    /// Gets the single active <see cref="IEventWriter{TAggregate}"/> for <typeparamref name="TAggregate"/> with the specified <paramref name="aggregateId"/>.
    /// </summary>
    /// <remarks>Enforces event writing for single aggregate to single writer.</remarks>
    ValueTask<IEventWriter<TAggregate>> GetActiveEventWriterForAsync(long aggregateId);
}
