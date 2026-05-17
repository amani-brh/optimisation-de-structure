using AmaniRobot.Domain;
using AmaniRobot.Domain.Accounts;
using AmaniRobot.Domain.Customers;
using AmaniRobot.Domain.Reports;
using AmaniRobot.Domain.ValueObjects;

namespace AmaniRobot.Infrastructure.PersistenceLayer.EntityFramework;

public sealed class EntityFactory : IEntityFactory
{
    public IAccount NewAccount(ICustomer customer)
    {
        var account = new Account(customer);
        return account;
    }

    public ICredit NewCredit(IAccount account, PositiveMoney amountToDeposit, DateTime transactionDate)
    {
        var credit = new Credit(account, amountToDeposit, transactionDate);
        return credit;
    }

    public ICustomer NewCustomer(SSN ssn, Name name)
    {
        var customer = new Customer(ssn, name);
        return customer;
    }

    public IDebit NewDebit(IAccount account, PositiveMoney amountToWithdraw, DateTime transactionDate)
    {
        var debit = new Debit(account, amountToWithdraw, transactionDate);
        return debit;
    }

    public IReport NewReport(int runId, DateTime timestamp, FilePath projectFile, Weight grandTotalKg, TotauxCollection totaux, RowsCollection rows)
    {
        throw new NotImplementedException();
    }

    public IReportRow NewReportRow(SectionType type, bool isHeader, int? nombre, double? lengthM, double? poidsUnitaire, double? poidsPiece, Weight? poidsTotal)
    {
        throw new NotImplementedException();
    }

    public ITotauxEntry NewTotauxEntry(SectionType material, Weight totalKg)
    {
        throw new NotImplementedException();
    }
}