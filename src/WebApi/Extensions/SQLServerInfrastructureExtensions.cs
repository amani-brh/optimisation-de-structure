using AmaniRobot.Application.Repositories;
using AmaniRobot.Application.Services;
using AmaniRobot.Domain;
using AmaniRobot.Infrastructure.PersistenceLayer.EntityFramework;
using AmaniRobot.Infrastructure.PersistenceLayer.EntityFramework.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AmaniRobot.WebApi.Extensions;

public static class SQLServerInfrastructureExtensions
{
    public static IServiceCollection AddSQLServerPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IEntityFactory, EntityFactory>();

        services.AddDbContext<GenocsContext>(
            options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();

        return services;
    }
}
