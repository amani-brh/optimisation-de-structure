namespace AmaniRobot.WebApi.UseCases.V1.Reports;

public class CreateOptimizationRequest
{
    public string AIRecommendation { get; set; } = string.Empty;

    public string OptimizationStrategy { get; set; } = string.Empty;

    public double CurrentWeightKg { get; set; }

    public double ProjectedWeightKg { get; set; }

    public List<string> DesignModifications { get; set; } = new();

    public List<string> ParameterSuggestions { get; set; } = new();

    public List<string> SuggestedModifications { get; set; } = new();

    public List<OptimizationResultRequest> Results { get; set; } = new();
}
