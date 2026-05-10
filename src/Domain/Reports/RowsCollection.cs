namespace AmaniRobot.Domain.Reports;

public class RowsCollection : List<IReportRow>
{
    /// <summary>All data rows (non-header).</summary>
    public IEnumerable<IReportRow> DataRows() =>
        this.Where(r => !r.IsHeader);

    /// <summary>All header/summary rows.</summary>
    public IEnumerable<IReportRow> HeaderRows() =>
        this.Where(r => r.IsHeader);
}