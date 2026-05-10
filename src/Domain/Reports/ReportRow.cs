using AmaniRobot.Domain.ValueObjects;

namespace AmaniRobot.Domain.Reports;

public class ReportRow : IReportRow
{
    public Guid Id { get; protected set; }
    public SectionType Type { get; protected set; }
    public bool IsHeader { get; protected set; }
    public int? Nombre { get; protected set; }
    public double? LengthM { get; protected set; }
    public double? PoidsUnitaire { get; protected set; }
    public double? PoidsPiece { get; protected set; }
    public Weight? PoidsTotal { get; protected set; }

    protected ReportRow()
    {
        Type = new SectionType("unknown");
    }

    public ReportRow(
        Guid id,
        SectionType type,
        bool isHeader,
        int? nombre,
        double? lengthM,
        double? poidsUnitaire,
        double? poidsPiece,
        Weight? poidsTotal)
    {
        Id = id;
        Type = type;
        IsHeader = isHeader;
        Nombre = nombre;
        LengthM = lengthM;
        PoidsUnitaire = poidsUnitaire;
        PoidsPiece = poidsPiece;
        PoidsTotal = poidsTotal;
    }
}