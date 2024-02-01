using Enqueuer.Queueing.Domain.Factories;
using Enqueuer.Queueing.Domain.Models;
using Enqueuer.Queueing.Domain.Repositories;
using Enqueuer.Queueing.Infrastructure.Messaging;
using Enqueuer.Queueing.Infrastructure.Persistence.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Enqueuer.Queueing.Infrastructure.Persistence.Repositories;

public class QueueRepository : IQueueRepository
{
    private readonly QueueingContext _context;
    private readonly IQueueFactory _queueFactory;
    private readonly IEventDispatcher _eventDispatcher;
    private static readonly string[] QueueIdProperties = ["Id"];
    private readonly Dictionary<int, Queue> _trackedQueues = new();

    public QueueRepository(
        QueueingContext context,
        IQueueFactory queueFactory,
        IEventDispatcher eventDispatcher)
    {
        _context = context;
        _queueFactory = queueFactory;
        _eventDispatcher = eventDispatcher;
    }

    public async Task<Queue> GetQueueAsync(int id, CancellationToken cancellationToken)
    {
        var queue = await _context.Queues.FindAsync(new object[] { id }, cancellationToken);
        if (queue == null)
        {
            throw new QueueDoesNotExistException($"Queue '{id}' does not exist in data storage.");
        }

        await _context.Entry(queue)
            .Collection(q => q.Participants).LoadAsync(cancellationToken);

        return _queueFactory.Create(
            queue.Id,
            queue.Name,
            queue.LocationId,
            queue.Participants.Select(p => new Participant(p.Id, p.Number)));
    }

    public Queue CreateNewQueue(string name, long locationId)
    {
        var queue = new Entities.Queue
        {
            Name = name,
            LocationId = locationId
        };

        _context.Queues.Add(queue);

        var domainModel = _queueFactory.CreateNew(queue.Id, name, locationId);

        _trackedQueues.TryAdd(queue.Id, domainModel);

        return domainModel;
    }

    public void UpdateQueue(Queue queue)
    {
        var storedQueueTracker = GetTrackedQueue(queue.Id);
        if (storedQueueTracker == null)
        {
            throw new InvalidOperationException("Cannot update non-tracked queue entity.");
        }

        _trackedQueues.TryAdd(queue.Id, queue);
        var storedQueue = storedQueueTracker.Entity;

        storedQueue.Name = queue.Name;
        storedQueue.Participants = queue.Participants.Select(p => new Entities.Participant
        {
            Id = p.Id,
            Number = p.Position.Number,
            QueueId = queue.Id
        }).ToList();

        if (storedQueueTracker.State == EntityState.Unchanged)
        {
            _context.Queues.Update(storedQueue);
        }
    }

    // TODO: reconsider the domain model, possibly introducing the "Location" entity/aggregate root
    public void DeleteQueue(Queue queue)
    {
        var storedQueueTracker = GetTrackedQueue(queue.Id);
        if (storedQueueTracker == null)
        {
            throw new InvalidOperationException("Cannot remove non-tracked queue entity.");
        }

        storedQueueTracker.State = EntityState.Deleted;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);

        var eventsToDispatch = _trackedQueues.Values
            .Where(q => q.DomainEvents.Any())
            .SelectMany(q => q.DomainEvents)
            .ToArray();

        await _eventDispatcher.DispatchEventsAsync(eventsToDispatch, cancellationToken);
    }

    private EntityEntry<Entities.Queue>? GetTrackedQueue(int queueId)
    {
        return _context.Queues.Local.FindEntry(QueueIdProperties, new object[] { queueId });
    }
}
