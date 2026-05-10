using System.Collections.ObjectModel;
using AmaniRobot.Application.Boundaries.CloseAccount;

namespace AmaniRobot.Infrastructure.PersistenceLayer.InMemory.Presenters;

public sealed class CloseAccountPresenter : IOutputPort
{
    public Collection<string> Errors { get; }
    public Collection<CloseAccountOutput> ClosedAccounts { get; }

    public CloseAccountPresenter()
    {
        Errors = new Collection<string>();
        ClosedAccounts = new Collection<CloseAccountOutput>();
    }

    public void Error(string message) => Errors.Add(message);

    public void Default(CloseAccountOutput output) => ClosedAccounts.Add(output);
}