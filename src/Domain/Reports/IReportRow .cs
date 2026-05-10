using AmaniRobot.Domain.ValueObjects;

namespace AmaniRobot.Domain.Reports;

public interface IReportRow : IEntity
{
    SectionType Type { get; }
    bool IsHeader { get; }
    int? Nombre { get; }
    double? LengthM { get; }
    double? PoidsUnitaire { get; }
    double? PoidsPiece { get; }
    Weight? PoidsTotal { get; }
}
