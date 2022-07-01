using ExampleBlazorApp.Shared;
using RandomSkunk.Results;

namespace ExampleBlazorApp.Server.Services;

public interface IWeatherForecastSimulator
{
    Task<Maybe<IReadOnlyList<WeatherForecast>>> GetFiveDayForecast(string city);
}
