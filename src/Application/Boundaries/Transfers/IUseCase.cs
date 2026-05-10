namespace AmaniRobot.Application.Boundaries.Transfers;

public interface IUseCase
{
    Task ExecuteAsync(TransferInput transferInput);
}