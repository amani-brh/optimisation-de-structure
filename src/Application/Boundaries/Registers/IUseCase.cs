namespace AmaniRobot.Application.Boundaries.Registers;

public interface IUseCase
{
    Task ExecuteAsync(RegisterInput registerInput);
}