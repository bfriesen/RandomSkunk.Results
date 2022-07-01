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

    public Task<Maybe<List<WeatherProfile>>> GetWeatherProfiles()
    {
        // Using the RandomSkunk.Results.Http package, make a request to the server to get the weather profiles.
        // Any errors from making the request are automatically captured in the result.
        return _httpClient.TryGetFromJsonAsync<List<WeatherProfile>>("WeatherProfiles");
    }

    public async Task<Result> AddWeatherProfile(WeatherProfile weatherProfile)
    {
        // Using the RandomSkunk.Results.Http package, make a request to the server to add the weather profile.
        // Any errors from making the request are automatically captured in the result.
        var responseResult = await _httpClient.TryPostAsJsonAsync("WeatherProfiles", weatherProfile);

        // Since the caller doesn't care about the actual HttpResponse, just make sure it has a success status code.
        return await responseResult.EnsureSuccessStatusCodeAsync();
    }

    public async Task<Result> EditWeatherProfile(WeatherProfile weatherProfile)
    {
        // Using the RandomSkunk.Results.Http package, make a request to the server to edit the weather profile.
        // Any errors from making the request are automatically captured in the result.
        var responseResult = await _httpClient.TryPutAsJsonAsync("WeatherProfiles", weatherProfile);

        // Since the caller doesn't care about the actual HttpResponse, just make sure it has a success status code.
        return await responseResult.EnsureSuccessStatusCodeAsync();
    }
}
