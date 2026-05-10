namespace AmaniRobot.Application.Boundaries.Refunds;

public interface IUseCase
{
    Task ExecuteAsync(RefundInput refundInput);
}