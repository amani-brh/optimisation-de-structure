using AmaniRobot.Domain.Reports;

namespace AmaniRobot.Application.Boundaries.Optimisatizer;

public sealed record CreateOptimizationOutput(Optimization Optimization);

public sealed record CreateOptimizationInput(
    Guid ReportId,
    OptimizationAnalysis Analysis,
    List<SectionModification> SuggestedModifications,
    OptimizationResults Results);
