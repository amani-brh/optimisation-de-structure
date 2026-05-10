using AmaniRobot.Domain.ValueObjects;

namespace AmaniRobot.Domain.Reports;

public class Report : IReport
{
    public Guid Id { get; protected set; }
    public int RunId { get; protected set; }
    public DateTime Timestamp { get; protected set; }
    public FilePath ProjectFile { get; protected set; }
    public Weight GrandTotalKg { get; protected set; }
    public TotauxCollection Totaux { get; protected set; }
    public RowsCollection Rows { get; protected set; }

    protected Report()
    {
        ProjectFile = new FilePath("unknown");
        GrandTotalKg = new Weight(0);
        Totaux = new TotauxCollection();
        Rows = new RowsCollection();
    }

    public Report(
        Guid id,
        int runId,
        DateTime timestamp,
        FilePath projectFile,
        Weight grandTotalKg,
        TotauxCollection totaux,
        RowsCollection rows)
    {
        Id = id;
        RunId = runId;
        Timestamp = timestamp;
        ProjectFile = projectFile;
        GrandTotalKg = grandTotalKg;
        Totaux = totaux;
        Rows = rows;
    }

    /// <summary>
    /// Convenience: data rows grouped by section type.
    /// </summary>
    public IEnumerable<IGrouping<string, IReportRow>> RowsBySection() =>
        Rows.DataRows().GroupBy(r => r.Type.Value);
}