using AmaniRobot.Domain.Accounts;
using AmaniRobot.Domain.ValueObjects;

namespace AmaniRobot.Application.Boundaries.Deposits;

public sealed class DepositOutput
{
    public Transaction Transaction { get; }
    public decimal UpdatedBalance { get; }

    public DepositOutput(
        ICredit credit,
        Money updatedBalance)
    {
        Credit creditEntity = (Credit)credit;

        Transaction = new Transaction(
                                    Credit.Description,
                                    creditEntity
                                    .Amount
                                    .ToMoney()
                                    .ToDecimal(),
                                    creditEntity.TransactionDate);

        UpdatedBalance = updatedBalance.ToDecimal();
    }
}