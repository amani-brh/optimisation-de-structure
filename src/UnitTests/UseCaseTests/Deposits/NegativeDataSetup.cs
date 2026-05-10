
using Xunit;

namespace AmaniRobot.UnitTests.UseCaseTests.Deposits;

internal sealed class NegativeDataSetup : TheoryData<decimal>
{
    public NegativeDataSetup()
    {
        Add(-100);
    }
}