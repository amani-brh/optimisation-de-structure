using AmaniRobot.Domain.ValueObjects;

namespace AmaniRobot.Domain.Reports;

public interface IReport : IAggregateRoot
{
    int RunId { get; }
    DateTime Timestamp { get; }
    FilePath ProjectFile { get; }
    Weight GrandTotalKg { get; }
    TotauxCollection Totaux { get; }
    RowsCollection Rows { get; }
}