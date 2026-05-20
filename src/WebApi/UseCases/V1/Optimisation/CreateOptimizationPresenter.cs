using AmaniRobot.Application.Boundaries.ImportReport;
using AmaniRobot.Application.Boundaries.Optimisatizer;
using Microsoft.AspNetCore.Mvc;
using IOutputPort = AmaniRobot.Application.Boundaries.Optimisatizer.IOutputPort;

namespace AmaniRobot.WebApi.UseCases.V1.Optimisation;

public sealed class CreateOptimizationPresenter : IOutputPort
{
    public IActionResult? ViewModel { get; private set; }

    public void Standard(CreateOptimizationOutput output)
    {
        ViewModel = new CreatedResult(
            $"/api/v1/Reports/{output.Optimization.ReportId}/optimizations/{output.Optimization.Id}",
            new
            {
                output.Optimization.Id,
                output.Optimization.ReportId,
                output.Optimization.Status,
                output.Optimization.CreatedAt,
                output.Optimization.Analysis.WeightReductionPercent,
            });
    }

    public void NotFound(string message) => ViewModel = new NotFoundObjectResult(new { message });
    public void Error(string message) => ViewModel = new BadRequestObjectResult(new { message });

    public void Standard(ImportReportOutput output)
    {
        throw new NotImplementedException();
    }
}
