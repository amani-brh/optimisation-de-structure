using AmaniRobot.Application.Boundaries.Optimisatizer.getAll;
using Microsoft.AspNetCore.Mvc;

namespace AmaniRobot.WebApi.UseCases.V1.Optimisation;

public sealed class GetAllOptimizationsPresenter : IOutputPort
{
    public IActionResult? ViewModel { get; private set; }

    public void Standard(GetAllOptimizationsOutput output)
    {
        var view = output.Optimizations.Select(o => new
        {
            o.Id,
            o.ReportId,
            o.Status,
            o.CreatedAt,
            o.Analysis.AIRecommendation,
            o.Analysis.OptimizationStrategy,
            o.Analysis.CurrentWeightKg,
            o.Analysis.ProjectedWeightKg,
            o.Analysis.WeightReductionPercent,
            o.Analysis.DesignModifications,
            o.Analysis.ParameterSuggestions,
            o.SuggestedModifications,
            o.Results,
        });
        ViewModel = new OkObjectResult(view);
    }

    public void NotFound(string message) => ViewModel = new NotFoundObjectResult(new { message });
    public void Error(string message) => ViewModel = new BadRequestObjectResult(new { message });
}