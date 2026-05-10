using AmaniRobot.Contracts.Interfaces;

namespace AmaniRobot.Contracts.Events;

public class WithdrawCompleted : IEvent
{
    public Guid AccountId { get; set; }
    public decimal Amount { get; set; }
}
