namespace AmaniRobot.WebApi.UseCases.V1.Reports;

public sealed record ReportRowResponse(
    string Type,
    bool IsHeader,
    int? Nombre,
    double? LengthM,
    double? PoidsUnitaire,
    double? PoidsPiece,
    decimal? PoidsTotal);
