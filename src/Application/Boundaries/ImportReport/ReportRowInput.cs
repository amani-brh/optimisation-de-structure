namespace AmaniRobot.Application.Boundaries.ImportReport;

public sealed record ReportRowInput(
    string Type,
    bool IsHeader,
    int? Nombre,
    double? LengthM,
    double? PoidsUnitaire,
    double? PoidsPiece,
    decimal? PoidsTotal);
