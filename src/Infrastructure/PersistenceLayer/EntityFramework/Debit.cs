using AmaniRobot.Domain.Accounts;
using AmaniRobot.Domain.ValueObjects;

namespace AmaniRobot.Infrastructure.PersistenceLayer.EntityFramework;

public class Debit : Domain.Accounts.Debit
{
    public Guid AccountId { get; protected set; }

    protected Debit()
    {
    }

    public Debit(IAccount account, PositiveMoney amountToWithdraw, DateTime transactionDate)
    {
        AccountId = account.Id;
        Amount = amountToWithdraw;
        TransactionDate = transactionDate;
    }
}