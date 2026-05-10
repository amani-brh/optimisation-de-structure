using AmaniRobot.Application.Services;

namespace AmaniRobot.Infrastructure.PersistenceLayer.InMemory;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly GenocsContext _context;

    public UnitOfWork(GenocsContext context)
        => _context = context;

    public async Task<int> Save()
        => await Task.FromResult(0);
}