
using Xunit;

namespace AmaniRobot.UnitTests.UseCaseTests.Transfers;

internal sealed class PositiveDataSetup : TheoryData<decimal, decimal>
{
    public PositiveDataSetup()
    {
        Add(100, 600);
    }
}