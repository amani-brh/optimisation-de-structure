using AmaniRobot.Application.Boundaries.ImportReport.GetReport;
using AmaniRobot.Application.Repositories;

namespace AmaniRobot.Application.UseCases;

public sealed class GetReport(
    IOutputPort outputPort,
    IReportRepository reportRepository)
{
    public async Task ExecuteAsync(GetReportInput input)
    {
        if (input is null)
        {
            outputPort.Error("Input is null.");
            return;
        }

        var report = await reportRepository.Get(input.Id);

        if (report is null)
        {
            outputPort.NotFound(input.Id);
            return;
        }

        outputPort.Standard(new GetReportOutput(report));
    }
}