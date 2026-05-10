using AmaniRobot.Contracts.Interfaces;

namespace AmaniRobot.Contracts.Events;

public class CloseAccountCompleted : IEvent
{
    public Guid AccountId { get; set; }
}
