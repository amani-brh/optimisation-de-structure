using AmaniRobot.Domain.Reports;
using AmaniRobot.Domain.ValueObjects;

namespace AmaniRobot.Infrastructure.PersistenceLayer.MongoDb;

public class Report : Domain.Reports.Report
{
    public Report() { }

    public Report(
        int runId,
        DateTime timestamp,
        FilePath projectFile,
        Weight grandTotalKg,
        TotauxCollection totaux,
        RowsCollection rows)
    {
        Id = Guid.NewGuid();
        RunId = runId;
        Timestamp = timestamp;
        ProjectFile = projectFile;
        GrandTotalKg = grandTotalKg;
        Totaux = totaux;
        Rows = rows;
    }
}
