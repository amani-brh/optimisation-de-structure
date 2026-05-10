using AmaniRobot.Application.Services;

namespace AmaniRobot.UnitTests.TestFixtures;

public class FakeServiceBus : IServiceBusClient
{
    public async Task PublishEventAsync<T>(T evt)
        where T : Contracts.Interfaces.IEvent
    {
        await Task.CompletedTask;
    }

    public async Task SendCommandAsync<T>(T cmd)
        where T : Contracts.Interfaces.ICommand
    {
        await Task.CompletedTask;
    }
}
