using AmaniRobot.Domain.Customers;

namespace AmaniRobot.Application.Repositories;

public interface ICustomerRepository
{
    Task<ICustomer> Get(Guid id);
    Task Add(ICustomer customer);
    Task Update(ICustomer customer);
}