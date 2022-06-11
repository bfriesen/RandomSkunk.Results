using ExampleBlazorApp.Shared;
using RandomSkunk.Results;
using RandomSkunk.Results.Http;

namespace ExampleBlazorApp.Client.HttpClients;

public class WeatherForecastClient
{
    private readonly HttpClient _httpClient;

    public WeatherForecastClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<Result<IReadOnlyList<WeatherForecast>>> GetFiveDayForecast(string city)
    {
        return _httpClient.TryGetFromJsonAsync<IReadOnlyList<WeatherForecast>>($"WeatherForecast/{city}")
            .AsResult();
    }
}
