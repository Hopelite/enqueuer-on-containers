namespace Enqueuer.Queueing.Infrastructure.Persistence.Storage;

public interface IEventWriterFactory
{
    IEventWriter GetEventWriterFor(long aggregateId, CancellationToken cancellationToken);
}
