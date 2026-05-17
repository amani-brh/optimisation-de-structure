using AmaniRobot.Application.Boundaries.ImportReport.GetAllReports;
using Microsoft.AspNetCore.Mvc;
using IOutputPort = AmaniRobot.Application.Boundaries.ImportReport.GetAllReports.IOutputPort;

namespace AmaniRobot.WebApi.UseCases.V1.Reports;

public sealed class GetAllReportsPresenter : IOutputPort
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

    public void Standard(GetAllReportsOutput output)
    {
        var response = output.Reports.Select(report => new ImportReportResponse(
            report.Id,
            report.RunId,
            report.Timestamp,
            report.ProjectFile.Value,
            report.GrandTotalKg.ToDecimal(),
            report.Totaux
                .Select(t => new TotauxEntryResponse(t.Material.Value, t.TotalKg.ToDecimal()))
                .ToList(),
            report.Rows
                .Select(r => new ReportRowResponse(
                    r.Type.Value,
                    r.IsHeader,
                    r.Nombre,
                    r.LengthM,
                    r.PoidsUnitaire,
                    r.PoidsPiece,
                    r.PoidsTotal?.ToDecimal()))
                .ToList()));

        ViewModel = new OkObjectResult(response);
    }
}