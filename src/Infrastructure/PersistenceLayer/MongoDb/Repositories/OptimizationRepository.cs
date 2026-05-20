using AmaniRobot.Application.Repositories;
using AmaniRobot.Domain.Reports;
using MongoDB.Driver;

namespace AmaniRobot.Infrastructure.PersistenceLayer.MongoDb.Repositories;

public sealed class OptimizationRepository : IOptimizationRepository
{
    private readonly IMongoContext _context;
    private readonly IMongoCollection<Optimization> _collection;

    public OptimizationRepository(IMongoContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _collection = _context.GetCollection<Optimization>("Optimizations");
    }

    public Task Add(Optimization optimization)
    {
        _context.AddCommand(async () =>
            await _collection.InsertOneAsync(optimization));
        return Task.CompletedTask;
    }

    public async Task<Optimization?> Get(Guid id)
    {
        var cursor = await _collection.FindAsync(o => o.Id == id);
        return await cursor.FirstOrDefaultAsync();
    }

    public async Task<List<Optimization>> GetByReportId(Guid reportId)
    {
        var cursor = await _collection.FindAsync(o => o.ReportId == reportId);
        return await cursor.ToListAsync();
    }

    public async Task Update(Optimization optimization)
        => await _collection.FindOneAndReplaceAsync(
            o => o.Id == optimization.Id,
            optimization);
    public async Task<List<Optimization>> GetAll()
    {
        var cursor = await _collection.FindAsync(FilterDefinition<Optimization>.Empty);
        return await cursor.ToListAsync();
    }
}