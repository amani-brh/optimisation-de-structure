using AmaniRobot.Domain.ValueObjects;

namespace AmaniRobot.Domain.Accounts;

public interface ICredit : IEntity
{
    PositiveMoney Sum(PositiveMoney amount);
}