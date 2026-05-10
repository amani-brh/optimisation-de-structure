namespace AmaniRobot.Application.Boundaries.Refunds;

public interface IOutputPort : IErrorHandler
{
    void Default(RefundOutput refundOutput);
}