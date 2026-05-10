using AmaniRobot.Domain.Accounts;
using AmaniRobot.Domain.ValueObjects;

namespace AmaniRobot.Application.Boundaries.Refunds;

public sealed class RefundOutput
{
    public Transaction Transaction { get; }
    public decimal UpdatedBalance { get; }

    public RefundOutput(IDebit debit, Money updatedBalance)
    {
        Debit debitEntity = (Debit)debit;

        Transaction = new Transaction(
            Debit.Description,
            debitEntity.Amount
            .ToMoney()
            .ToDecimal(),
            debitEntity.TransactionDate);

        UpdatedBalance = updatedBalance.ToDecimal();
    }
}