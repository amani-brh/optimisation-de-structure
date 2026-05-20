using AmaniRobot.Domain.Reports;

namespace AmaniRobot.Application.Boundaries.Optimisatizer;

public sealed record GetAllOptimizationsOutput(IReadOnlyList<Optimization> Optimizations);
