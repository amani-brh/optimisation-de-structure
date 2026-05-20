namespace AmaniRobot.WebApi.UseCases.V1.Reports;

public class OptimizationResultRequest
{
    public string Parameter { get; set; } = string.Empty;

    public string Before { get; set; } = string.Empty;

    public string After { get; set; } = string.Empty;
}