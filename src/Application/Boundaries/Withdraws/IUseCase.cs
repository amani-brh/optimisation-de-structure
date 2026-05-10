namespace AmaniRobot.Application.Boundaries.Withdraws;

public interface IUseCase
{
    Task ExecuteAsync(WithdrawInput withdrawInput);
}