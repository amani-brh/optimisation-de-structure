using AmaniRobot.Application.Services;
using AmaniRobot.Contracts.ReadModels;
using AmaniRobot.Infrastructure.WebApiClient.Exceptions;

namespace AmaniRobot.Infrastructure.WebApiClient.ExternalServices;

public class DummyApiClient : ApiClient, IDummyApiClient
{
    public DummyApiClient(HttpClient httpClient)
        : base(httpClient)
    {
    }

    public async Task<SimpleResult> GetSimpleModelAsync(string id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/Dummy/{id}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<SimpleResult>();
            }

            throw new BackendServiceCallFailedException(response.ReasonPhrase);
        }
        catch (BackendServiceCallFailedException)
        {
            throw;
        }
        catch (Exception e)
        {
            throw new BackendServiceCallFailedException(e.Message, e);
        }
    }
}
