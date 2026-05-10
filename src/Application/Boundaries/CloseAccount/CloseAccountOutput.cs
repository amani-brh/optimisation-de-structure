using AmaniRobot.Domain.Accounts;

namespace AmaniRobot.Application.Boundaries.CloseAccount;

public sealed class CloseAccountOutput(IAccount account)
{
    public Guid AccountId { get; } = account.Id;
}