namespace Enqueuer.Queueing.Infrastructure.Persistence.Storage.Writing;

public interface IEventWriterFactory<TAggregate>
{
    /// <summary>
    /// Creates new <see cref="IEventWriter{TAggregate}"/> for <typeparamref name="TAggregate"/> with the specified <paramref name="aggregateId"/>.
    /// </summary>
    IEventWriter<TAggregate> CreateEventWriterFor(long aggregateId);
}
