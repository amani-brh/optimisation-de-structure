using AmaniRobot.Domain.ValueObjects;

namespace AmaniRobot.Infrastructure.PersistenceLayer.MongoDb;

public class TotauxEntry : Domain.Reports.TotauxEntry
{
    public TotauxEntry() { }

    public TotauxEntry(SectionType material, Weight totalKg)
    {
        Id = Guid.NewGuid();
        Material = material;
        TotalKg = totalKg;
    }
}