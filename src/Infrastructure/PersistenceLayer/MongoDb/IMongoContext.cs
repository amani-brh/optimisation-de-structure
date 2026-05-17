using AmaniRobot.Domain;
using MongoDB.Driver;

namespace AmaniRobot.Infrastructure.PersistenceLayer.MongoDb;

public interface IMongoContext : IDisposable
{
    MongoClient MongoClient { get; set; }
    IClientSessionHandle Session { get; set; }
    Task<int> SaveChangesAsync();
    void AddCommand(Func<Task> func);

    // existing — for domain entities
    IMongoCollection<T> GetCollection<T>(string name) where T : IEntity;

    // new — for flat document models
    IMongoCollection<T> GetDocumentCollection<T>(string name);

}