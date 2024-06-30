using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Enqueuer.Telegram.Notifications.Services.Factories;

public class MongoClientFactory(IOptions<MongoDatabaseSettings> databaseSettings) : IMongoClientFactory
{
    private readonly MongoDatabaseSettings _databaseSettings = databaseSettings.Value;

    public IMongoCollection<T> GetCollection<T>(string collectionName)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(collectionName, nameof(collectionName));

        var mongoClient = new MongoClient(_databaseSettings.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(_databaseSettings.DatabaseName);

        return mongoDatabase.GetCollection<T>(collectionName);
    }
}
