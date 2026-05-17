using AmaniRobot.Domain.ValueObjects;

namespace AmaniRobot.Infrastructure.PersistenceLayer.MongoDb;

public class ReportRow : Domain.Reports.ReportRow
{
    public ReportRow() { }

    public ReportRow(
        SectionType type,
        bool isHeader,
        int? nombre,
        double? lengthM,
        double? poidsUnitaire,
        double? poidsPiece,
        Weight? poidsTotal)
    {
        Id = Guid.NewGuid();
        Type = type;
        IsHeader = isHeader;
        Nombre = nombre;
        LengthM = lengthM;
        PoidsUnitaire = poidsUnitaire;
        PoidsPiece = poidsPiece;
        PoidsTotal = poidsTotal;
    }
}