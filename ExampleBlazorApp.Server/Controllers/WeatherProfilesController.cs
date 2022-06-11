using ExampleBlazorApp.Server.Repositories;
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
    public async Task<IActionResult> GetWeatherProfiles()
    {
        return await _weatherRepository.GetWeatherProfiles().ToActionResult();
    }
}
