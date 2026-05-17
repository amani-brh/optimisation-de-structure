using AmaniRobot.Application.Boundaries.ImportReport;
using AmaniRobot.Application.Boundaries.ImportReport.GetReport;
using AmaniRobot.Application.Repositories;
using AmaniRobot.Application.Services;
using AmaniRobot.Domain;
using AmaniRobot.Domain.Reports;
using AmaniRobot.Domain.ValueObjects;
using IOutputPort = AmaniRobot.Application.Boundaries.ImportReport.IOutputPort;
using IUseCase = AmaniRobot.Application.Boundaries.ImportReport.IUseCase;

namespace AmaniRobot.Application.UseCases;

public sealed class ImportReport(
    IEntityFactory entityFactory,
    IOutputPort outputPort,
    IReportRepository reportRepository,
    IUnitOfWork unitOfWork)
    : IUseCase
{
    public async Task ExecuteAsync(ImportReportInput input)
    {
        if (input is null)
        {
            outputPort.Error("Input is null.");
            return;
        }

        // Guard: reject duplicate run_id
        var existing = await reportRepository.GetByRunId(input.RunId);
        if (existing is not null)
        {
            outputPort.Error($"A report with run_id {input.RunId} already exists.");
            return;
        }

        // Build TotauxCollection
        var totaux = new TotauxCollection();
        foreach (var t in input.Totaux)
        {
            var entry = entityFactory.NewTotauxEntry(
                new SectionType(t.Material),
                new Weight(t.TotalKg));
            totaux.Add(entry);
        }

        // Build RowsCollection
        var rows = new RowsCollection();
        foreach (var r in input.Rows)
        {
            var row = entityFactory.NewReportRow(
                new SectionType(r.Type),
                r.IsHeader,
                r.Nombre,
                r.LengthM,
                r.PoidsUnitaire,
                r.PoidsPiece,
                r.PoidsTotal.HasValue ? new Weight(r.PoidsTotal.Value) : null);
            rows.Add(row);
        }

        // Assemble aggregate
        var report = entityFactory.NewReport(
            input.RunId,
            input.Timestamp,
            new FilePath(input.ProjectFile),
            new Weight(input.GrandTotalKg),
            totaux,
            rows);

        await reportRepository.Add(report);
        await unitOfWork.Save();

        outputPort.Standard(new ImportReportOutput(report));
    }
}
