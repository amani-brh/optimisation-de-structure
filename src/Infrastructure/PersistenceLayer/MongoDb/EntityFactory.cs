using AmaniRobot.Domain;
using AmaniRobot.Domain.Accounts;
using AmaniRobot.Domain.Customers;
using AmaniRobot.Domain.Reports;
using AmaniRobot.Domain.ValueObjects;

namespace AmaniRobot.Infrastructure.PersistenceLayer.MongoDb;

public sealed class EntityFactory : IEntityFactory
{
    // ── existing ──────────────────────────────────────────────────

    public IAccount NewAccount(ICustomer customer)
        => new Account(customer);

    public ICredit NewCredit(IAccount account, PositiveMoney amountToDeposit, DateTime transactionDate)
        => new Credit(account, amountToDeposit, transactionDate);

    public ICustomer NewCustomer(SSN ssn, Name name)
        => new Customer(ssn, name);

    public IDebit NewDebit(IAccount account, PositiveMoney amountToWithdraw, DateTime transactionDate)
        => new Debit(account, amountToWithdraw, transactionDate);

    // ── robot report ──────────────────────────────────────────────

    public IReport NewReport(
        int runId,
        DateTime timestamp,
        FilePath projectFile,
        Weight grandTotalKg,
        TotauxCollection totaux,
        RowsCollection rows)
        => new Report(runId, timestamp, projectFile, grandTotalKg, totaux, rows);

    public ITotauxEntry NewTotauxEntry(SectionType material, Weight totalKg)
        => new TotauxEntry(material, totalKg);

    public IReportRow NewReportRow(
        SectionType type,
        bool isHeader,
        int? nombre,
        double? lengthM,
        double? poidsUnitaire,
        double? poidsPiece,
        Weight? poidsTotal)
        => new ReportRow(type, isHeader, nombre, lengthM, poidsUnitaire, poidsPiece, poidsTotal);
}