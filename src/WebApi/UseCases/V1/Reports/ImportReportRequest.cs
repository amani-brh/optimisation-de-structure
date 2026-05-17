namespace AmaniRobot.WebApi.UseCases.V1.Reports;

public sealed record ImportReportRequest(
    int RunId,
    DateTime Timestamp,
    string ProjectFile,
    decimal GrandTotalKg,
    IReadOnlyList<TotauxEntryRequest> Totaux,
    IReadOnlyList<ReportRowRequest> Rows);
