using Enqueuer.Queueing.Domain.Events;
using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace Enqueuer.Queueing.Infrastructure.Persistence.Storage;

public class DocumentEventStorage : IEventStorage
{
    private readonly IMongoCollection<DomainEvent> _eventCollection;

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
        return await (await _eventCollection.FindAsync(e => e.AggregateId == aggregateId, cancellationToken: cancellationToken))
            .ToListAsync(cancellationToken);
    }

    public Task WriteEventsAsync(IEnumerable<DomainEvent> events, CancellationToken cancellationToken)
    {
        return _eventCollection.InsertManyAsync(events, cancellationToken: cancellationToken);
    }
}
