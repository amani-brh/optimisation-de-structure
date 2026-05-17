// WebApi/UseCases/V1/Reports/ImportReportPresenter.cs
using AmaniRobot.Application.Boundaries.ImportReport;  // ← correct IOutputPort
using Microsoft.AspNetCore.Mvc;

namespace AmaniRobot.WebApi.UseCases.V1.Reports;

public sealed class ImportReportPresenter : IOutputPort  // ← fully resolves to ImportReport.IOutputPort
{
    public IActionResult? ViewModel { get; private set; }

    public void Error(string message)
    {
        ViewModel = new BadRequestObjectResult(new ProblemDetails
        {
            Title = "An error occurred",
            Detail = message
        });
    }

    public void Standard(ImportReportOutput output)
    {
        var totaux = output.Report.Totaux
            .Select(t => new TotauxEntryResponse(
                t.Material.Value,
                t.TotalKg.ToDecimal()))
            .ToList();

        var rows = output.Report.Rows
            .Select(r => new ReportRowResponse(
                r.Type.Value,
                r.IsHeader,
                r.Nombre,
                r.LengthM,
                r.PoidsUnitaire,
                r.PoidsPiece,
                r.PoidsTotal?.ToDecimal()))
            .ToList();

        ViewModel = new CreatedAtRouteResult(
            "GetReport",
            new { reportId = output.Report.Id },
            new ImportReportResponse(
                output.Report.Id,
                output.Report.RunId,
                output.Report.Timestamp,
                output.Report.ProjectFile.Value,
                output.Report.GrandTotalKg.ToDecimal(),
                totaux,
                rows));
    }
}