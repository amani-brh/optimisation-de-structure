using AmaniRobot.Contracts.ReadModels;

namespace AmaniRobot.Application.Services;

public interface IAuthApiClient : IApiClient
{
    Task<SimpleResult> GetSimpleAuthModelAsync(string id);
}
