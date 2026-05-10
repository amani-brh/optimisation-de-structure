
using Xunit;

namespace AmaniRobot.UnitTests.UseCaseTests.CloseAccounts;

internal sealed class PositiveDataSetup : TheoryData<decimal>
{
    public PositiveDataSetup()
    {
        Add(0.5M);
        Add(100M);
        Add(200M);
    }
}