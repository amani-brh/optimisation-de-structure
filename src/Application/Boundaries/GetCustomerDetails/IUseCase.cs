namespace AmaniRobot.Application.Boundaries.GetCustomerDetails;

public interface IUseCase
{
    Task ExecuteAsync(GetCustomerDetailsInput getCustomerDetailsInput);
}