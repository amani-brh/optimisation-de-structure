using System.Collections.ObjectModel;
using AmaniRobot.Application.Boundaries.Registers;

namespace AmaniRobot.Infrastructure.PersistenceLayer.InMemory.Presenters;

public sealed class RegisterPresenter : IOutputPort
{
    public Collection<string> Errors { get; }
    public Collection<RegisterOutput> Registers { get; }

    public RegisterPresenter()
    {
        Errors = new Collection<string>();
        Registers = new Collection<RegisterOutput>();
    }

    public void Error(string message)
    {
        Errors.Add(message);
    }

    public void Standard(RegisterOutput output)
    {
        Registers.Add(output);
    }
}