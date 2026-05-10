using AmaniRobot.Application.Boundaries.Deposits;
using AmaniRobot.Application.UseCases;
using AmaniRobot.Domain.Exceptions;
using AmaniRobot.Domain.ValueObjects;
using AmaniRobot.Infrastructure.PersistenceLayer.InMemory.Presenters;
using AmaniRobot.UnitTests.TestFixtures;
using Xunit;

namespace AmaniRobot.UnitTests.UseCaseTests.Deposits;

public sealed class DepositTests : IClassFixture<StandardFixture>
{
    private readonly StandardFixture _fixture;
    public DepositTests(StandardFixture fixture)
    {
        _fixture = fixture;
    }

    [Theory]
    [ClassData(typeof(PositiveDataSetup))]
    public async Task Deposit_ChangesBalance(decimal amount)
    {
        var presenter = new DepositPresenter();
        var sut = new Deposit(
            _fixture.EntityFactory,
            presenter,
            _fixture.AccountRepository,
            _fixture.UnitOfWork,
            _fixture.ServiceBus
        );

        await sut.ExecuteAsync(
            new DepositInput(
                _fixture.Context.DefaultAccountId,
                new PositiveMoney(amount)));

        var output = presenter.Deposits.Last();
        Assert.Equal(amount, output.Transaction.Amount);
    }

    [Theory]
    [ClassData(typeof(NegativeDataSetup))]
    public async Task Deposit_ShouldNot_ChangesBalance_WhenNegative(decimal amount)
    {
        var presenter = new DepositPresenter();
        var sut = new Deposit(
            _fixture.EntityFactory,
            presenter,
            _fixture.AccountRepository,
            _fixture.UnitOfWork,
            _fixture.ServiceBus);

        await Assert.ThrowsAsync<MoneyShouldBePositiveException>(() =>
            sut.ExecuteAsync(new DepositInput(_fixture.Context.DefaultAccountId, new PositiveMoney(amount))));
    }
}