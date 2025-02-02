using MongoDB.Driver;

namespace Enqueuer.Telegram.Notifications.Services.Factories;

public interface IMongoClientFactory
{
    /// <summary>
    /// Gets <see cref="IMongoCollection{TDocument}"/> with the specified <paramref name="collectionName"/>.
    /// </summary>
    IMongoCollection<T> GetCollection<T>(string collectionName);
}
