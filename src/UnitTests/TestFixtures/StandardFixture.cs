using AmaniRobot.Application.Services;
using AmaniRobot.Infrastructure.PersistenceLayer.InMemory;
using AmaniRobot.Infrastructure.PersistenceLayer.InMemory.Repositories;

namespace AmaniRobot.UnitTests.TestFixtures;

public sealed class StandardFixture
{
    public EntityFactory EntityFactory { get; }
    public GenocsContext Context { get; }
    public AccountRepository AccountRepository { get; }
    public CustomerRepository CustomerRepository { get; }
    public UnitOfWork UnitOfWork { get; }

    public IServiceBusClient ServiceBus { get; }

    public StandardFixture()
    {
        Context = new GenocsContext();
        AccountRepository = new AccountRepository(Context);
        CustomerRepository = new CustomerRepository(Context);
        UnitOfWork = new UnitOfWork(Context);
        EntityFactory = new EntityFactory();
        ServiceBus = new FakeServiceBus();
    }
}