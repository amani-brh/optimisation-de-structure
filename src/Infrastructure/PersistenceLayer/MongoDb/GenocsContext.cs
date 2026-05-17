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

    public MongoClient MongoClient { get; set; }
    public IClientSessionHandle Session { get; set; }

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

    public async Task<int> SaveChangesAsync()
    {
        int count = _commands.Count;
        CancellationToken token = new CancellationToken();

        using (Session = await MongoClient.StartSessionAsync(options: null, cancellationToken: token))
        {
            Session.StartTransaction();
            var commandTasks = _commands.Select(c => c());
            await Task.WhenAll(commandTasks);
            await Session.CommitTransactionAsync();
            _commands.Clear();
            Session.Dispose();
            Session = null!;
        }

        return count;
    }

    private bool _disposed = false;

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public void AddCommand(Func<Task> func)
        => _commands.Add(func);

    public IMongoCollection<T> GetCollection<T>(string name)
        where T : IEntity
    {
        // Use the settings with custom serializers registered
        var settings = new MongoCollectionSettings();
        return _database.GetCollection<T>(name, settings);
    }
    public IMongoCollection<T> GetDocumentCollection<T>(string name)
        => _database.GetCollection<T>(name);
    private void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            _disposed = true;
            if (disposing)
            {
                while (Session != null && Session.IsInTransaction)
                    Thread.Sleep(TimeSpan.FromMilliseconds(100));
            }
        }
    }

    public static void RegisterConventions()
    {
        // Register custom serializers BEFORE registering class maps
        BsonSerializer.RegisterSerializer(new ObjectIdToGuidSerializer());
        
        var pack = new ConventionPack
        {
            new IgnoreExtraElementsConvention(true),
            new IgnoreIfDefaultConvention(true)
        };

        ConventionRegistry.Register("Genocs Solution Conventions", pack, t => true);

        // Register Report class map AFTER serializer registration
        RegisterReportClassMap();
    }

    private static void RegisterReportClassMap()
    {
        if (!BsonClassMap.IsClassMapRegistered(typeof(Report)))
        {
            BsonClassMap.RegisterClassMap<Report>(classMap =>
            {
                classMap.AutoMap();
            });
        }
    }
}
