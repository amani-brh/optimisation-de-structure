using AmaniRobot.Domain.Accounts;
using AmaniRobot.Domain.ValueObjects;

namespace AmaniRobot.Infrastructure.PersistenceLayer.InMemory;

public class Credit : Domain.Accounts.Credit
{
    public Guid AccountId { get; protected set; }

    protected Credit() { }

    public Credit(IAccount account, PositiveMoney amountToDeposit, DateTime transactionDate)
    {
        AccountId = account.Id;
        Amount = amountToDeposit;
        TransactionDate = transactionDate;
    }
}