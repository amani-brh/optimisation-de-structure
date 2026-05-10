namespace AmaniRobot.Application.Boundaries.Deposits;

public interface IUseCase
{
    Task ExecuteAsync(DepositInput depositInput);
}