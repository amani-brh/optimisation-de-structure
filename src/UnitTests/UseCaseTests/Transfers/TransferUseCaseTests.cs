using AmaniRobot.Application.Boundaries.Transfers;
using AmaniRobot.Application.UseCases;
using AmaniRobot.Domain.ValueObjects;
using AmaniRobot.Infrastructure.PersistenceLayer.InMemory.Presenters;
using AmaniRobot.UnitTests.TestFixtures;
using Xunit;

namespace AmaniRobot.UnitTests.UseCaseTests.Transfers;

public sealed class TransferUseCaseTests : IClassFixture<StandardFixture>
{
    private readonly StandardFixture _fixture;
    public TransferUseCaseTests(StandardFixture fixture)
    {
        _fixture = fixture;
    }

    [Theory]
    [ClassData(typeof(PositiveDataSetup))]
    public async Task Transfer_ChangesBalance_WhenAccountExists(
        decimal amount,
        decimal expectedOriginBalance)
    {
        var presenter = new TransferPresenter();
        var sut = new Transfer(
            _fixture.EntityFactory,
            presenter,
            _fixture.AccountRepository,
            _fixture.UnitOfWork,
            _fixture.ServiceBus
        );

        await sut.ExecuteAsync(
            new TransferInput(
                _fixture.Context.DefaultAccountId,
                _fixture.Context.SecondAccountId,
                new PositiveMoney(amount)));

        var actual = presenter.Transfers.Last();
        Assert.Equal(expectedOriginBalance, actual.UpdatedBalance);
    }
}