using AmaniRobot.Domain.Accounts;
using AmaniRobot.Domain.Customers;
using AmaniRobot.Domain.Reports;
using AmaniRobot.Domain.ValueObjects;

namespace AmaniRobot.Domain;

public interface IEntityFactory
{
    // ── existing ──────────────────────────────────────────────────
    ICustomer NewCustomer(SSN ssn, Name name);
    IAccount NewAccount(ICustomer customer);
    ICredit NewCredit(IAccount account, PositiveMoney amountToDeposit, DateTime transactionDate);
    IDebit NewDebit(IAccount account, PositiveMoney amountToWithdraw, DateTime transactionDate);

    // ── robot report ──────────────────────────────────────────────
    IReport NewReport(int runId, DateTime timestamp, FilePath projectFile,
                           Weight grandTotalKg, TotauxCollection totaux, RowsCollection rows);

    ITotauxEntry NewTotauxEntry(SectionType material, Weight totalKg);

    IReportRow NewReportRow(SectionType type, bool isHeader,
                              int? nombre, double? lengthM,
                              double? poidsUnitaire, double? poidsPiece,
                              Weight? poidsTotal);
}