using AmaniRobot.Domain;
using AmaniRobot.Domain.Reports;
using AmaniRobot.Infrastructure.PersistenceLayer.MongoDb.Serializers;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace AmaniRobot.Infrastructure.PersistenceLayer.MongoDb;

public sealed class GenocsContext : IMongoContext
{
    private readonly IMongoDatabase _database;
    private readonly List<Func<Task>> _commands;
    private bool _disposed = false;

    public MongoClient MongoClient { get; set; }
    public IClientSessionHandle Session { get; set; } = null!;

    public GenocsContext(IConfiguration configuration)
    {
        _commands = new List<Func<Task>>();

        MongoClient = new MongoClient(
            Environment.GetEnvironmentVariable("MONGOCONNECTION")
            ?? configuration.GetSection("MongoSettings").GetSection("Connection").Value);

        _database = MongoClient.GetDatabase(
            Environment.GetEnvironmentVariable("DATABASENAME")
            ?? configuration.GetSection("MongoSettings").GetSection("DatabaseName").Value);
    }

    /// <summary>
    /// Executes all queued commands. No transaction wrapper - standalone
    /// MongoDB does not support multi-document transactions.
    /// </summary>
    public async Task<int> SaveChangesAsync()
    {
        int count = _commands.Count;
        if (count == 0) return 0;

        var commandTasks = _commands.Select(c => c());
        await Task.WhenAll(commandTasks);
        _commands.Clear();
        return count;
    }

    public void AddCommand(Func<Task> func) => _commands.Add(func);

    public IMongoCollection<T> GetCollection<T>(string name) where T : IEntity
        => _database.GetCollection<T>(name, new MongoCollectionSettings());

    public IMongoCollection<T> GetDocumentCollection<T>(string name)
        => _database.GetCollection<T>(name);

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (_disposed) return;
        _disposed = true;
        if (disposing)
        {
            Session?.Dispose();
            Session = null!;
        }
    }

    public static void RegisterConventions()
    {
        BsonSerializer.RegisterSerializer(new ObjectIdToGuidSerializer());

        var pack = new ConventionPack
        {
            new IgnoreExtraElementsConvention(true),
            new IgnoreIfDefaultConvention(true)
        };
        ConventionRegistry.Register("Genocs Solution Conventions", pack, t => true);

        RegisterReportClassMap();
        RegisterOptimizationClassMap();
    }

    private static void RegisterReportClassMap()
    {
        if (!BsonClassMap.IsClassMapRegistered(typeof(Report)))
        {
            BsonClassMap.RegisterClassMap<Report>(cm => cm.AutoMap());
        }
    }

    private static void RegisterOptimizationClassMap()
    {
        if (!BsonClassMap.IsClassMapRegistered(typeof(Optimization)))
        {
            BsonClassMap.RegisterClassMap<Optimization>(cm =>
            {
                cm.AutoMap();
                cm.MapIdProperty(o => o.Id);
            });
        }
        if (!BsonClassMap.IsClassMapRegistered(typeof(OptimizationAnalysis)))
            BsonClassMap.RegisterClassMap<OptimizationAnalysis>(cm => cm.AutoMap());
        if (!BsonClassMap.IsClassMapRegistered(typeof(SectionModification)))
            BsonClassMap.RegisterClassMap<SectionModification>(cm => cm.AutoMap());
        if (!BsonClassMap.IsClassMapRegistered(typeof(OptimizationResults)))
            BsonClassMap.RegisterClassMap<OptimizationResults>(cm => cm.AutoMap());
    }
}