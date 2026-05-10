using AmaniRobot.Contracts.Interfaces;

namespace AmaniRobot.Contracts.Commands;

public class SimpleMessage : ICommand
{
    public string? MessageId { get; set; }

    public string? MessageBody { get; set; }
}
