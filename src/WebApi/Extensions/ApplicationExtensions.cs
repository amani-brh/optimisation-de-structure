using AmaniRobot.Application.Services;
using AmaniRobot.Application.UseCases;
using AmaniRobot.Infrastructure.WebApiClient.ExternalServices;

namespace AmaniRobot.WebApi.Extensions;

public static class ApplicationExtensions
{
    public static IServiceCollection AddUseCases(this IServiceCollection services)
    {

        services.AddScoped<IApiClient, AuthApiClient>();

        services.AddScoped<Application.Boundaries.CloseAccount.IUseCase, CloseAccount>();
        services.AddScoped<Application.Boundaries.Deposits.IUseCase, Deposit>();
        services.AddScoped<Application.Boundaries.GetAccountDetails.IUseCase, GetAccountDetails>();
        services.AddScoped<Application.Boundaries.Refunds.IUseCase, Refund>();
        services.AddScoped<Application.Boundaries.GetCustomerDetails.IUseCase, GetCustomerDetails>();
        services.AddScoped<Application.Boundaries.Registers.IUseCase, Register>();
        services.AddScoped<Application.Boundaries.Withdraws.IUseCase, Withdraw>();
        services.AddScoped<Application.Boundaries.Transfers.IUseCase, Transfer>();
        return services;
    }
}
