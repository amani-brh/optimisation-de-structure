using AmaniRobot.Domain.Reports;

namespace AmaniRobot.Application.Boundaries.Optimisatizer.getAll;

public sealed record GetAllOptimizationsOutput(IReadOnlyList<Optimization> Optimizations);
