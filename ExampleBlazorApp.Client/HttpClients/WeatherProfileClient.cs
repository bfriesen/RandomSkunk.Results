using ExampleBlazorApp.Shared;
using RandomSkunk.Results;
using RandomSkunk.Results.Http;

namespace ExampleBlazorApp.Client.HttpClients;

public class WeatherProfileClient
{
    private readonly HttpClient _httpClient;

    public WeatherProfileClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<Result<List<WeatherProfile>>> GetWeatherProfiles()
    {
        return _httpClient.TryGetFromJsonAsync<List<WeatherProfile>>("WeatherProfiles")
            .AsResult();
    }

    public Task<Result> AddWeatherProfile(WeatherProfile weatherProfile)
    {
        return _httpClient.TryPostAsJsonAsync("WeatherProfiles", weatherProfile)
            .EnsureSuccessStatusCodeAsync();
    }

    public Task<Result> EditWeatherProfile(WeatherProfile weatherProfile)
    {
        return _httpClient.TryPutAsJsonAsync("WeatherProfiles", weatherProfile)
            .EnsureSuccessStatusCodeAsync();
    }
}
