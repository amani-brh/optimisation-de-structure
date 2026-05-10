using AmaniRobot.Domain.Accounts;

namespace AmaniRobot.Domain.Customers;

public interface ICustomer : IAggregateRoot
{
    AccountCollection Accounts { get; }
    void Register(IAccount account);
}