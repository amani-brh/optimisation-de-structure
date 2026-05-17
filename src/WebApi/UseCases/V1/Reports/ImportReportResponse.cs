namespace AmaniRobot.WebApi.UseCases.V1.Reports;

public sealed record ImportReportResponse(
    Guid ReportId,
    int RunId,
    DateTime Timestamp,
    string ProjectFile,
    decimal GrandTotalKg,
    IReadOnlyList<TotauxEntryResponse> Totaux,
    IReadOnlyList<ReportRowResponse> Rows);
