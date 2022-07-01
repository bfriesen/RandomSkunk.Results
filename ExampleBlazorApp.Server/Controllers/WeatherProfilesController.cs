using ExampleBlazorApp.Server.Repositories;
using ExampleBlazorApp.Shared;
using Microsoft.AspNetCore.Mvc;
using RandomSkunk.Results.AspNetCore;

namespace ExampleBlazorApp.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherProfilesController : ControllerBase
{
    private readonly IWeatherRepository _weatherRepository;

    public WeatherProfilesController(IWeatherRepository weatherRepository)
    {
        _weatherRepository = weatherRepository;
    }

    [HttpGet]
    public Task<IActionResult> GetWeatherProfiles()
    {
        // Get the list of weather profiles from the database as a Result<IReadOnlyList<WeatherProfile>>
        // using our WeatherRepository.
        return _weatherRepository.GetWeatherProfiles()

            // Using the RandomSkunk.Results.AspNetCore package, convert the
            // Result<IReadOnlyList<WeatherProfile>> into an equivalent IActionResult.
            .ToActionResult();
    }

    [HttpPost]
    public Task<IActionResult> AddWeatherProfile([FromBody] WeatherProfile weatherProfile)
    {
        // Add the weather profile to the database using our WeatherRepository.
        return _weatherRepository.AddWeatherProfile(weatherProfile)

            // Using the RandomSkunk.Results.AspNetCore package, convert the Result into an
            // equivalent IActionResult.
            .ToActionResult();
    }

    [HttpPut]
    public Task<IActionResult> EditWeatherProfile([FromBody] WeatherProfile weatherProfile)
    {
        // Edit the weather profile to the database using our WeatherRepository.
        return _weatherRepository.EditWeatherProfile(weatherProfile)

            // Using the RandomSkunk.Results.AspNetCore package, convert the Result into an
            // equivalent IActionResult.
            .ToActionResult();
    }
}
