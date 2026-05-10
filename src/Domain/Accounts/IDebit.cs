using AmaniRobot.Domain.ValueObjects;

namespace AmaniRobot.Domain.Accounts;

public interface IDebit : IEntity
{
    PositiveMoney Sum(PositiveMoney amount);
}