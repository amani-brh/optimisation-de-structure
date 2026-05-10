namespace AmaniRobot.Application.Services;

public interface IServiceBusClient
{
    Task SendCommandAsync<T>(T cmd)
        where T : Contracts.Interfaces.ICommand;

    Task PublishEventAsync<T>(T evt)
        where T : Contracts.Interfaces.IEvent;
}