using AmaniRobot.Domain.ValueObjects;

namespace AmaniRobot.Domain.Reports;

public class TotauxEntry : ITotauxEntry
{
    public Guid Id { get; protected set; }
    public SectionType Material { get; protected set; }
    public Weight TotalKg { get; protected set; }

    protected TotauxEntry()
    {
        Material = new SectionType("unknown");
        TotalKg = new Weight(0);
    }

    public TotauxEntry(Guid id, SectionType material, Weight totalKg)
    {
        Id = id;
        Material = material;
        TotalKg = totalKg;
    }
}