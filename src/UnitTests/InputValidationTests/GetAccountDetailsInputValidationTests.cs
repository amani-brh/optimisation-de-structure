using AmaniRobot.Application.Boundaries.GetAccountDetails;
using AmaniRobot.Application.Exceptions;
using Xunit;

namespace AmaniRobot.UnitTests.InputValidationTests;

public sealed class GetAccountDetailsInputValidationTests
{
    [Fact]
    public void GivenEmptyAccountId_InputNotCreated_ThrowsInputValidationException()
    {
        var actualEx = Assert.Throws<InputValidationException>(
            () => new GetAccountDetailsInput(
                Guid.Empty
            ));
        Assert.Contains("accountId", actualEx.Message);
    }

    [Fact]
    public void GivenValidData_InputCreated()
    {
        var actual = new GetAccountDetailsInput(
            Guid.NewGuid()
        );
        Assert.NotNull(actual);
    }
}