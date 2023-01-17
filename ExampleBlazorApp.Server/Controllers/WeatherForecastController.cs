using ExampleBlazorApp.Server.Services;
using ExampleBlazorApp.Shared;
using Microsoft.AspNetCore.Mvc;
using RandomSkunk.Results;
using RandomSkunk.Results.AspNetCore;

namespace ExampleBlazorApp.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly IWeatherForecastSimulator _weatherForecastSimulator;

    public WeatherForecastController(IWeatherForecastSimulator weatherForecastSimulator)
    {
        _weatherForecastSimulator = weatherForecastSimulator;
    }

    [HttpGet("{city}")]
    public async Task<IActionResult> Get(string city)
    {
        // Get a simulated five day forcast as a Maybe<IReadOnlyList<WeatherForcast>>
        // using our WeatherForecastSimulator.
        Maybe<IReadOnlyList<WeatherForecast>> forcastResult =
            await _weatherForecastSimulator.GetFiveDayForecast(city);

        // Using the RandomSkunk.Results.AspNetCore package, convert the
        // Maybe<IReadOnlyList<WeatherForcast>> into an equivalent IActionResult.
        IActionResult actionResult = forcastResult.ToActionResult();
        return actionResult;
    }
}
