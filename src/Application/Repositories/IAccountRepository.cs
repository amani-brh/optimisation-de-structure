using AmaniRobot.Domain.Accounts;

namespace AmaniRobot.Application.Repositories;

public interface IAccountRepository
{
    Task<IAccount> Get(Guid id);
    Task Add(IAccount account, ICredit credit);
    Task Update(IAccount account, ICredit credit);
    Task Update(IAccount account, IDebit debit);
    Task Delete(IAccount account);
}
