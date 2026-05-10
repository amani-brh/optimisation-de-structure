namespace AmaniRobot.Application.Boundaries.CloseAccount;

public interface IUseCase
{
    Task ExecuteAsync(CloseAccountInput closeAccountInput);
}