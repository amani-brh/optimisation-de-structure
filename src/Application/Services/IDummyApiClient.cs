using AmaniRobot.Contracts.ReadModels;

namespace AmaniRobot.Application.Services;

public interface IDummyApiClient : IApiClient
{
    Task<SimpleResult> GetSimpleModelAsync(string id);
}
