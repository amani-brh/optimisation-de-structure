using AmaniRobot.Domain.ValueObjects;

namespace AmaniRobot.Domain.Accounts;

public class Debit : IDebit
{
    public Guid Id { get; protected set; }

    public PositiveMoney Amount { get; protected set; } = new PositiveMoney(0);

    public static string Description => "Debit";

    public DateTime TransactionDate { get; protected set; }

    public PositiveMoney Sum(PositiveMoney amount)
    {
        return Amount.Add(amount);
    }
}