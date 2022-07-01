using ExampleBlazorApp.Server.Services;
using Microsoft.AspNetCore.Mvc;
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
        return await _weatherForecastSimulator.GetFiveDayForecast(city)

            // Using the RandomSkunk.Results.AspNetCore package, convert the
            // Maybe<IReadOnlyList<WeatherForcast>> into an equivalent IActionResult.
            .ToActionResult();
    }
}
