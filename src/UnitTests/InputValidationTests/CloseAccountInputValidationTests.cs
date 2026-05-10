using AmaniRobot.Application.Boundaries.CloseAccount;
using AmaniRobot.Application.Exceptions;
using Xunit;

namespace AmaniRobot.UnitTests.InputValidationTests;

public sealed class CloseAccountInputValidationTests
{
    [Fact]
    public void GivenEmptyAccountId_InputNotCreated_ThrowsInputValidationException()
    {
        var actualEx = Assert.Throws<InputValidationException>(
            () => new CloseAccountInput(
                Guid.Empty
            ));
        Assert.Contains("accountId", actualEx.Message);
    }

    [Fact]
    public void GivenValidData_InputCreated()
    {
        var actual = new CloseAccountInput(
            Guid.NewGuid()
        );
        Assert.NotNull(actual);
    }
}