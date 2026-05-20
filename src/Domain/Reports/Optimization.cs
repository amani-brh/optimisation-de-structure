namespace AmaniRobot.Domain.Reports;

public class Optimization : IEntity
{
    public Guid Id { get; protected set; }
    public Guid ReportId { get; protected set; }
    public DateTime CreatedAt { get; protected set; }
    public OptimizationStatus Status { get; protected set; }
    public OptimizationAnalysis Analysis { get; protected set; }
    public List<SectionModification> SuggestedModifications { get; protected set; }
    public OptimizationResults Results { get; protected set; }

    protected Optimization() { }

    public Optimization(
        Guid id,
        Guid reportId,
        OptimizationAnalysis analysis,
        List<SectionModification> modifications,
        OptimizationResults results)
    {
        Id = id;
        ReportId = reportId;
        CreatedAt = DateTime.UtcNow;
        Status = OptimizationStatus.Completed;
        Analysis = analysis;
        SuggestedModifications = modifications;
        Results = results;
    }
}

public enum OptimizationStatus
{
    Pending,
    Processing,
    Completed,
    Failed,
    Approved,
    Rejected
}

public class OptimizationAnalysis
{
    public string AIRecommendation { get; set; }
    public string OptimizationStrategy { get; set; }
    public double CurrentWeightKg { get; set; }
    public double ProjectedWeightKg { get; set; }
    public double WeightReductionPercent { get; set; }
    public List<string> DesignModifications { get; set; }
    public Dictionary<string, string> ParameterSuggestions { get; set; }

    public OptimizationAnalysis() { }

    public OptimizationAnalysis(
        string recommendation,
        string strategy,
        double currentWeight,
        double projectedWeight,
        List<string> modifications,
        Dictionary<string, string> parameters)
    {
        AIRecommendation = recommendation;
        OptimizationStrategy = strategy;
        CurrentWeightKg = currentWeight;
        ProjectedWeightKg = projectedWeight;
        WeightReductionPercent = ((currentWeight - projectedWeight) / currentWeight) * 100;
        DesignModifications = modifications;
        ParameterSuggestions = parameters;
    }
}

public class SectionModification
{
    public string SectionType { get; set; }
    public string CurrentProfile { get; set; }
    public string RecommendedProfile { get; set; }
    public int Quantity { get; set; }
    public double CurrentWeightPerPiece { get; set; }
    public double RecommendedWeightPerPiece { get; set; }
    public double TotalWeightSavings { get; set; }
    public string Justification { get; set; }
}

public class OptimizationResults
{
    public double TotalWeightSavings { get; set; }
    public double EstimatedCostImpact { get; set; }
    public int NumberOfSectionsModified { get; set; }
    public string FeasibilityAssessment { get; set; }
    public List<string> ImplementationNotes { get; set; }
}