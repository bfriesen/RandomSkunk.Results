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
        return _weatherRepository.GetWeatherProfiles().ToActionResult();
    }

    [HttpPost]
    public Task<IActionResult> AddWeatherProfile([FromBody] WeatherProfile weatherProfile)
    {
        return _weatherRepository.AddWeatherProfile(weatherProfile).ToActionResult();
    }

    [HttpPut]
    public Task<IActionResult> EditWeatherProfile([FromBody] WeatherProfile weatherProfile)
    {
        return _weatherRepository.EditWeatherProfile(weatherProfile).ToActionResult();
    }
}
