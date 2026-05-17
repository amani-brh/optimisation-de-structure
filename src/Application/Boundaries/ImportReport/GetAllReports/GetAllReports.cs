using AmaniRobot.Application.Repositories;

namespace AmaniRobot.Application.Boundaries.ImportReport.GetAllReports;

public sealed class GetAllReports(
    IOutputPort outputPort,
    IReportRepository reportRepository)
{
    public async Task ExecuteAsync()
    {
        var reports = await reportRepository.GetAll();
        outputPort.Standard(new GetAllReportsOutput(reports));
    }
}