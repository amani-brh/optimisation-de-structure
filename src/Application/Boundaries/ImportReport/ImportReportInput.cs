namespace AmaniRobot.Application.Boundaries.ImportReport;

public sealed record ImportReportInput(
    int RunId,
    DateTime Timestamp,
    string ProjectFile,
    decimal GrandTotalKg,
    IReadOnlyList<TotauxEntryInput> Totaux,
    IReadOnlyList<ReportRowInput> Rows);
