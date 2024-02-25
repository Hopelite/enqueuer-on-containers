using Enqueuer.Queueing.Domain.Models;
using System.Collections.Concurrent;

namespace Enqueuer.Queueing.Infrastructure.Persistence.Storage;

public class EventWriterManager : IEventWriterManager<Group>, IDisposable
{
    private readonly ConcurrentDictionary<long, IEventWriter<Group>> _activeWriters;
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly IEventWriterFactory<Group> _writerFactory;

    public EventWriterManager(IEventWriterFactory<Group> writerFactory)
    {
        _activeWriters = new ConcurrentDictionary<long, IEventWriter<Group>>();
        _cancellationTokenSource = new CancellationTokenSource();
        _writerFactory = writerFactory;
    }

    public async ValueTask<IEventWriter<Group>> GetActiveEventWriterForAsync(long aggregateId)
    {
        if (_activeWriters.TryGetValue(aggregateId, out var writer))
        {
            return writer;
        }

        // TODO: consider adding Timeouts for writers
        writer = _writerFactory.CreateEventWriterFor(aggregateId);
        await writer.StartAsync(_cancellationTokenSource.Token);
        if (!_activeWriters.TryAdd(aggregateId, writer))
        {
            // TODO: Consider usage of locks to avoid situation when writer is disposed
            // due to inactivity right at the moment when code reaches this block
            await writer.StopAsync(_cancellationTokenSource.Token);
            return _activeWriters[aggregateId];
        }

        return writer;
    }

    public void Dispose()
    {
        _cancellationTokenSource.Cancel();
    }
}
