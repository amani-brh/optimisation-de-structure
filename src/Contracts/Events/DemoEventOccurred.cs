using AmaniRobot.Contracts.Interfaces;

namespace AmaniRobot.Contracts.Events;

public class DemoEventOccurred : IIntegrationEvent
{
    public string? Payload { get; set; }
    public int Value { get; set; }
}
