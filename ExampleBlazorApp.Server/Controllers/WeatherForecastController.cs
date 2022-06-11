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
        return await _weatherForecastSimulator.GetFiveDayForecast(city).ToActionResult();
    }
}
