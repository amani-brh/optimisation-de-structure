using AmaniRobot.Application.Repositories;
using AmaniRobot.Application.Services;
using AmaniRobot.Domain;
using AmaniRobot.Infrastructure.PersistenceLayer.InMemory;
using AmaniRobot.Infrastructure.PersistenceLayer.InMemory.Repositories;

namespace AmaniRobot.WebApi.Extensions;

public static class InMemoryInfrastructureExtensions
{
    public static IServiceCollection AddInMemoryPersistence(this IServiceCollection services)
    {
        services.AddScoped<IEntityFactory, EntityFactory>();

        services.AddSingleton<GenocsContext, GenocsContext>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();


        return services;
    }
}
