using AmaniRobot.Application.Boundaries.ImportReport.GetAllReports;
using AmaniRobot.Application.Boundaries.ImportReport.GetReport; 
using Microsoft.AspNetCore.Mvc;
using IOutputPort = AmaniRobot.Application.Boundaries.ImportReport.GetReport.IOutputPort;

namespace AmaniRobot.WebApi.UseCases.V1.Reports;

public sealed class GetReportPresenter : IOutputPort  
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

    public void NotFound(Guid id)
    {
        ViewModel = new NotFoundObjectResult(new ProblemDetails
        {
            Title = "Report not found",
            Detail = $"No report found with id '{id}'."
        });
    }

    public void Standard(GetReportOutput output)
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

        ViewModel = new OkObjectResult(new ImportReportResponse(
            output.Report.Id,
            output.Report.RunId,
            output.Report.Timestamp,
            output.Report.ProjectFile.Value,
            output.Report.GrandTotalKg.ToDecimal(),
            totaux,
            rows));
    }
}
