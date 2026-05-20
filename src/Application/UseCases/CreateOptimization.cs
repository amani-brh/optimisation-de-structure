using AmaniRobot.Application.Boundaries.Optimisatizer;
using AmaniRobot.Application.Repositories;
using AmaniRobot.Application.Services;
using AmaniRobot.Domain.Reports;

namespace AmaniRobot.Application.UseCases;

public sealed class CreateOptimization(
    IOutputPort outputPort,
    IReportRepository reportRepository,
    IOptimizationRepository optimizationRepository,
    IUnitOfWork unitOfWork)
{
    public async Task ExecuteAsync(CreateOptimizationInput input)
    {
        // Validate report exists
        //var report = await reportRepository.Get(input.ReportId);
        //if (report is null)
        //{
        //    outputPort.NotFound($"Report {input.ReportId} not found");
        //    return;
        //}

        // Create optimization aggregate
        var optimization = new Optimization(
            Guid.NewGuid(),
            input.ReportId,
            input.Analysis,
            input.SuggestedModifications,
            input.Results);

        await optimizationRepository.Add(optimization);
        await unitOfWork.Save();

        outputPort.Standard(new CreateOptimizationOutput(optimization));
    }
}