namespace AmaniRobot.Application.Boundaries.Withdraws;

public interface IOutputPort : IErrorHandler
{
    void Default(WithdrawOutput withdrawOutput);
}