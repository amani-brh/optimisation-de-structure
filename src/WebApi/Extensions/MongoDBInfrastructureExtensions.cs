using AmaniRobot.Application.Repositories;
using AmaniRobot.Application.Services;
using AmaniRobot.Domain;
using AmaniRobot.Infrastructure.PersistenceLayer.MongoDb;
using AmaniRobot.Infrastructure.PersistenceLayer.MongoDb.Repositories;

namespace AmaniRobot.WebApi.Extensions;

public static class MongoDBInfrastructureExtensions
{
    public static IServiceCollection AddMongoDBPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        // Initialize the static conventions
        GenocsContext.RegisterConventions();

        services.AddScoped<IEntityFactory, EntityFactory>();
        services.AddScoped<IMongoContext, GenocsContext>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();

        return services;
    }
}
