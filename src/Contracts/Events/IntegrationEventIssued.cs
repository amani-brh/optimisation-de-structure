using AmaniRobot.Contracts.Interfaces;

namespace AmaniRobot.Contracts.Events;

public class IntegrationEventIssued : IIntegrationEvent
{
    public string? Title { get; set; }
}
