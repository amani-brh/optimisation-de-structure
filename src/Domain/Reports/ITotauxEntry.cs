using AmaniRobot.Domain.ValueObjects;

namespace AmaniRobot.Domain.Reports;

public interface ITotauxEntry : IEntity
{
    SectionType Material { get; }
    Weight TotalKg { get; }
}
