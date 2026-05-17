// WebApi/Extensions/UserInterfaceV1Extensions.cs
using AmaniRobot.Application.Boundaries.ImportReport.GetAllReports;
using AmaniRobot.Application.Repositories;
using AmaniRobot.Infrastructure.PersistenceLayer.MongoDb.Repositories;
using AmaniRobot.WebApi.UseCases.V1.CloseAccount;
using AmaniRobot.WebApi.UseCases.V1.Deposit;
using AmaniRobot.WebApi.UseCases.V1.GetAccountDetails;
using AmaniRobot.WebApi.UseCases.V1.GetCustomerDetails;
using AmaniRobot.WebApi.UseCases.V1.Refund;
using AmaniRobot.WebApi.UseCases.V1.Register;
using AmaniRobot.WebApi.UseCases.V1.Reports;
using AmaniRobot.WebApi.UseCases.V1.Transfer;
using AmaniRobot.WebApi.UseCases.V1.Withdraw;

namespace AmaniRobot.WebApi.Extensions;

public static class UserInterfaceV1Extensions
{
    public static IServiceCollection AddPresentersV1(this IServiceCollection services)
    {
        services.AddScoped<CloseAccountPresenter, CloseAccountPresenter>();
        services.AddScoped<Application.Boundaries.CloseAccount.IOutputPort>(x => x.GetRequiredService<CloseAccountPresenter>());
        services.AddScoped<DepositPresenter, DepositPresenter>();
        services.AddScoped<Application.Boundaries.Deposits.IOutputPort>(x => x.GetRequiredService<DepositPresenter>());
        services.AddScoped<GetAccountDetailsPresenter, GetAccountDetailsPresenter>();
        services.AddScoped<Application.Boundaries.GetAccountDetails.IOutputPort>(x => x.GetRequiredService<GetAccountDetailsPresenter>());
        services.AddScoped<GetCustomerDetailsPresenter, GetCustomerDetailsPresenter>();
        services.AddScoped<Application.Boundaries.GetCustomerDetails.IOutputPort>(x => x.GetRequiredService<GetCustomerDetailsPresenter>());
        services.AddScoped<RegisterPresenter, RegisterPresenter>();
        services.AddScoped<Application.Boundaries.Registers.IOutputPort>(x => x.GetRequiredService<RegisterPresenter>());
        services.AddScoped<WithdrawPresenter, WithdrawPresenter>();
        services.AddScoped<Application.Boundaries.Withdraws.IOutputPort>(x => x.GetRequiredService<WithdrawPresenter>());
        services.AddScoped<RefundPresenter, RefundPresenter>();
        services.AddScoped<Application.Boundaries.Refunds.IOutputPort>(x => x.GetRequiredService<RefundPresenter>());
        services.AddScoped<TransferPresenter, TransferPresenter>();
        services.AddScoped<Application.Boundaries.Transfers.IOutputPort>(x => x.GetRequiredService<TransferPresenter>());

        // Report Presenters — all fully qualified, no aliases, no boundary usings
        services.AddScoped<ImportReportPresenter, ImportReportPresenter>();
        services.AddScoped<Application.Boundaries.ImportReport.IOutputPort>(
            x => x.GetRequiredService<ImportReportPresenter>());

        services.AddScoped<GetReportPresenter, GetReportPresenter>();
        services.AddScoped<Application.Boundaries.ImportReport.GetReport.IOutputPort>(
            x => x.GetRequiredService<GetReportPresenter>());

        services.AddScoped<GetAllReportsPresenter, GetAllReportsPresenter>();
        services.AddScoped<Application.Boundaries.ImportReport.GetAllReports.IOutputPort>(
            x => x.GetRequiredService<GetAllReportsPresenter>());

        // Use cases — fully qualified to avoid collision with boundary namespace names
        services.AddScoped<Application.UseCases.ImportReport>();
        services.AddScoped<Application.UseCases.GetReport>();
        services.AddScoped<GetAllReports>();

        // Repository
        services.AddScoped<IReportRepository, ReportRepository>();

        return services;
    }
}