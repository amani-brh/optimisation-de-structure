namespace AmaniRobot.Application.Boundaries.GetAccountDetails;

public interface IUseCase
{
    Task ExecuteAsync(GetAccountDetailsInput getAccountDetailsInput);
}