using AmaniRobot.Domain.ValueObjects;

namespace AmaniRobot.Domain.Reports;

public class TotauxCollection : List<ITotauxEntry>
{
    public Weight GetGrandTotal()
    {
        return this.Aggregate(
            new Weight(0),
            (acc, entry) => acc.Add(entry.TotalKg));
    }
}