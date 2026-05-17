using AmaniRobot.Domain.Reports;

namespace AmaniRobot.Application.Boundaries.ImportReport.GetAllReports;

public sealed record GetAllReportsOutput(IEnumerable<IReport> Reports);
