using Enqueuer.Queueing.Domain.Events;
using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace Enqueuer.Queueing.Infrastructure.Persistence.Storage;

public class DocumentEventStorage : IEventStorage
{
    private readonly IMongoCollection<DomainEvent> _eventCollection;

    // TODO: refactor events serialization registration
    static DocumentEventStorage()
    {
        BsonClassMap.RegisterClassMap<DomainEvent>(c =>
        {
            c.SetIsRootClass(true);
            c.MapMember(e => e.AggregateId);
            c.MapMember(e => e.Timestamp);
            c.UnmapMember(e => e.Name);
        });

        BsonClassMap.RegisterClassMap<QueueCreatedEvent>(c =>
        {
            c.MapMember(e => e.QueueName);
            c.MapCreator(e => new QueueCreatedEvent(e.AggregateId, e.QueueName, e.Timestamp));
        });

        BsonClassMap.RegisterClassMap<QueueDeletedEvent>(c =>
        {
            c.MapMember(e => e.QueueName);
            c.MapCreator(e => new QueueDeletedEvent(e.AggregateId, e.QueueName, e.Timestamp));
        });

        BsonClassMap.RegisterClassMap<ParticipantEnqueuedEvent>(c =>
        {
            c.MapMember(e => e.QueueName);
            c.MapMember(e => e.ParticipantId);
            c.MapCreator(e => new ParticipantEnqueuedEvent(e.AggregateId, e.QueueName, e.ParticipantId, e.Timestamp));
        });

        BsonClassMap.RegisterClassMap<ParticipantEnqueuedAtEvent>(c =>
        {
            c.MapMember(e => e.QueueName);
            c.MapMember(e => e.ParticipantId);
            c.MapMember(e => e.Position);
            c.MapCreator(e => new ParticipantEnqueuedAtEvent(e.AggregateId, e.QueueName, e.ParticipantId, e.Position, e.Timestamp));
        });

        BsonClassMap.RegisterClassMap<ParticipantDequeuedEvent>(c =>
        {
            c.MapMember(e => e.QueueName);
            c.MapMember(e => e.ParticipantId);
            c.MapCreator(e => new ParticipantDequeuedEvent(e.AggregateId, e.QueueName, e.ParticipantId, e.Timestamp));
        });
    }

    public DocumentEventStorage(IOptions<EventsDatabaseSettings> databaseSettings)
    {
        var mongoClient = new MongoClient(
            databaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            databaseSettings.Value.DatabaseName);

        _eventCollection = mongoDatabase.GetCollection<DomainEvent>(
            databaseSettings.Value.EventsCollectionName);
    }

    public async Task<IEnumerable<DomainEvent>> GetAggregateEventsAsync(long aggregateId, CancellationToken cancellationToken)
    {
        return await _eventCollection.Find(e => e.AggregateId == aggregateId)
            .SortBy(e => e.Timestamp)
            .ToListAsync(cancellationToken);
    }

    public Task WriteEventsAsync(IEnumerable<DomainEvent> events, CancellationToken cancellationToken)
    {
        return _eventCollection.InsertManyAsync(events, cancellationToken: cancellationToken);
    }
}
