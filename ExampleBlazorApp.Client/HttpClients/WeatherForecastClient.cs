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

    public async Task<Result<IReadOnlyList<WeatherForecast>>> GetFiveDayForecast(string city)
    {
        string requestUri = $"WeatherForecast/{city}";

        // Using the RandomSkunk.Results.Http package, make a request to the server to get the five-day forecast.
        // Any errors from making the request are automatically captured in the result.
        Result<IReadOnlyList<WeatherForecast>> forcastResult =
            await _httpClient.TryGetFromJsonAsync<IReadOnlyList<WeatherForecast>>(requestUri);
        return forcastResult;
    }
}
