namespace AmaniRobot.Application.Boundaries.CloseAccount;

public interface IOutputPort : IErrorHandler
{
    void Default(CloseAccountOutput closeAccountOutput);
}